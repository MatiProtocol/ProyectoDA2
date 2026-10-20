using Domain;

namespace Abstraction;

public interface IPointsHistoryService
{
    void RecordTransaction(int visitorParkInfoId, int points, string origin, string description, string activeStrategy, int? relatedEntityId = null);
    List<PointsHistory> GetVisitorHistory(int visitorId);
    List<PointsHistory> GetHistoryByDateRange(int visitorId, DateTime startDate, DateTime endDate);
}
