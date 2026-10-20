using Abstraction;
using Domain;

namespace Services;

public class PointsHistoryService(
    IPointsHistoryRepository pointsHistoryRepository,
    IVisitorService visitorService)
    : IPointsHistoryService
{
    public void RecordTransaction(
        int visitorParkInfoId,
        int points,
        string origin,
        string description,
        string activeStrategy,
        int? relatedEntityId = null)
    {
        var pointsHistory = new PointsHistory
        {
            VisitorParkInformationId = visitorParkInfoId,
            Points = points,
            Origin = origin,
            Description = description,
            ActiveStrategy = activeStrategy,
            RelatedEntityId = relatedEntityId,
            TransactionDate = DateTime.Now
        };

        pointsHistoryRepository.Add(pointsHistory);
    }

    public List<PointsHistory> GetVisitorHistory(int visitorId)
    {
        var visitor = visitorService.GetVisitorWithParkInformation(visitorId);
        if(visitor == null)
        {
            throw new ArgumentException($"Visitor with id {visitorId} not found");
        }

        return pointsHistoryRepository.GetByVisitorParkInfoId(visitor.VisitorParkInformation.Id);
    }

    public List<PointsHistory> GetHistoryByDateRange(int visitorId, DateTime startDate, DateTime endDate)
    {
        var visitor = visitorService.GetVisitorWithParkInformation(visitorId);
        if(visitor == null)
        {
            throw new ArgumentException($"Visitor with id {visitorId} not found");
        }

        if(startDate > endDate)
        {
            throw new ArgumentException("Start date cannot be greater than end date");
        }

        return pointsHistoryRepository.GetByDateRange(
            visitor.VisitorParkInformation.Id,
            startDate,
            endDate);
    }
}
