using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IPointsHistoryRepository : IRepository<PointsHistory>
{
    List<PointsHistory> GetByVisitorParkInfoId(int visitorParkInfoId);
    List<PointsHistory> GetByDateRange(int visitorParkInfoId, DateTime startDate, DateTime endDate);
    int GetTotalPointsByVisitor(int visitorParkInfoId);
}
