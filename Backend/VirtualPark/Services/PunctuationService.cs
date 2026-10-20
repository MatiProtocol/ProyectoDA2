using Abstraction;
using Domain;
using NonDomainAbstractions;

namespace Services;

public class PunctuationService : IPunctuationService
{
    private readonly Dictionary<string, IPunctuationStrategy> _strategies;
    private static string _activeStrategyName = null!;
    private readonly IVisitorService _visitorService;
    private readonly IDateTimeService _dateTimeService;
    private readonly IPointsHistoryService _pointsHistoryService;
    private readonly StrategyLoader _strategyLoader;

    public PunctuationService(
        IVisitorService visitorService,
        IDateTimeService dateTimeService,
        IPointsHistoryService pointsHistoryService,
        IServiceProvider serviceProvider)
    {
        _visitorService = visitorService;
        _dateTimeService = dateTimeService;
        _pointsHistoryService = pointsHistoryService;
        _strategies = new Dictionary<string, IPunctuationStrategy>
        {
            { "AttractionBased", new AttractionPunctuation() },
            { "Combo", new ComboPunctuation() },
            { "Event", new EventPunctuation() }
        };

        _strategyLoader = new StrategyLoader(_strategies, serviceProvider);
        _activeStrategyName ??= "AttractionBased";
    }

    public string GetActiveStrategy()
    {
        return _activeStrategyName;
    }

    public List<string> GetAvailableStrategies()
    {
        return _strategies.Keys.ToList();
    }

    public void SetActiveStrategy(string strategyName)
    {
        if(string.IsNullOrWhiteSpace(strategyName))
        {
            throw new ArgumentException("Strategy name cannot be null or empty");
        }

        if(!_strategies.ContainsKey(strategyName))
        {
            throw new ArgumentException($"Strategy '{strategyName}' not found");
        }

        _activeStrategyName = strategyName;
    }

    public int CalculatePoints(PunctuationContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var activeStrategy = _strategies[_activeStrategyName];
        return activeStrategy.CalculatePoints(context);
    }

    public int GetVisitorTotalPoints(int? visitorId, string? visitorNfc)
    {
        if(visitorId == null)
        {
            visitorId = _visitorService.GetAll()
                .FirstOrDefault(v => v.VisitorParkInformation.NFC.Equals(Guid.Parse(visitorNfc!)))!.Id;
        }

        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId.Value);
        if(visitor == null)
        {
            throw new ArgumentException($"Visitor with id {visitorId} not found");
        }

        return visitor.VisitorParkInformation.Points;
    }

    public void AddPointsToVisitor(int visitorId, int points)
    {
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);
        if(visitor == null)
        {
            throw new ArgumentException($"Visitor with id {visitorId} not found");
        }

        visitor.VisitorParkInformation.Points += points;
        visitor.VisitorParkInformation.TodayPoints += points;

        _visitorService.UpdateVisitorParkInformation(visitor.VisitorParkInformation);
    }

    public Dictionary<string, int> GetDailyRanking()
    {
        var ranking = new Dictionary<string, int>();
        var allVisitors = _visitorService.GetAll(null);

        foreach(var visitor in allVisitors)
        {
            if(!HasTicketForToday(visitor))
            {
                continue;
            }

            var visitorWithParkInfo = _visitorService.GetVisitorWithParkInformation(visitor.Id);

            ResetVisitorTodayPoints(visitorWithParkInfo);

            var visitorName = $"{visitor.Name} - {visitor.Email}";
            ranking[visitorName] = visitorWithParkInfo.VisitorParkInformation.TodayPoints;
        }

        return ranking
            .OrderByDescending(kvp => kvp.Value)
            .Take(10)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public int ProcessAttractionPoints(int? visitorId, int attractionId, string? visitorNfc)
    {
        if(visitorId == null)
        {
            visitorId = _visitorService.GetAll()
                .FirstOrDefault(v => v.VisitorParkInformation.NFC.Equals(Guid.Parse(visitorNfc!)))!.Id;
        }

        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId.Value);
        if(visitor == null)
        {
            throw new ArgumentException($"Visitor with id {visitorId} not found");
        }

        if(ResetVisitorTodayPoints(visitor))
        {
            return 0;
        }

        var visitorParkInformation = visitor.VisitorParkInformation;

        var actualVisitedAttractionId = visitorParkInformation.VisitedAttractions
            .OrderByDescending(v => v.AttractionId);

        var visitedAttraction = actualVisitedAttractionId.FirstOrDefault(va => va.AttractionId == attractionId);
        if(visitedAttraction == null)
        {
            throw new ArgumentException($"Visitor with id {visitorId} not found");
        }

        var context = new PunctuationContext
        {
            VisitorParkInformationId = visitorParkInformation.Id,
            ActualAttraction = visitedAttraction,
            PreviousAccesses = visitorParkInformation.VisitedAttractions
                .Where(va => va.VisitDate.Date == _dateTimeService.Now.Date && va.AttractionId != visitedAttraction.AttractionId)
                .Cast<IContextVisitedAttraction>()
                .ToList()
        };

        var points = CalculatePoints(context);
        AddPointsToVisitor(visitorId.Value, points);

        _pointsHistoryService.RecordTransaction(
            visitorParkInformation.Id,
            points,
            "Attraction",
            $"Completed attraction ID: {attractionId}",
            _activeStrategyName,
            attractionId);

        return points;
    }

    private bool HasTicketForToday(Visitor visitor)
    {
        var today = _dateTimeService.Now.Date;

        return visitor.VisitorParkInformation.Tickets
            .Any(t => t.Date.Date.Equals(today));
    }

    private bool ResetVisitorTodayPoints(Visitor visitor)
    {
        var parkInfo = visitor.VisitorParkInformation;

        var lastVisitedAttraction = parkInfo.VisitedAttractions
            .OrderByDescending(a => a.VisitDate).FirstOrDefault();

        if(lastVisitedAttraction == null)
        {
            return false;
        }

        if(lastVisitedAttraction.VisitDate.Date >= _dateTimeService.Now.Date)
        {
            return false;
        }

        visitor.VisitorParkInformation.TodayPoints = 0;
        _visitorService.UpdateVisitorParkInformation(parkInfo);
        return true;
    }
}
