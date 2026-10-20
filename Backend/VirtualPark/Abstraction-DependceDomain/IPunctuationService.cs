using Domain;

namespace Abstraction;

public interface IPunctuationService
{
    void SetActiveStrategy(string strategyName);
    string GetActiveStrategy();
    List<string> GetAvailableStrategies();
    int CalculatePoints(PunctuationContext context);
    int GetVisitorTotalPoints(int? visitorId, string? visitorNfc);
    void AddPointsToVisitor(int visitorId, int points);
    Dictionary<string, int> GetDailyRanking();
    int ProcessAttractionPoints(int? visitorId, int attractionId, string? visitorNfc);
}
