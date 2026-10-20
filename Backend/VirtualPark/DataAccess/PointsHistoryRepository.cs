using Abstraction;
using Domain;

namespace DataAccess;

public class PointsHistoryRepository(SqlContext context) : Repository<PointsHistory>(context), IPointsHistoryRepository
{
    public List<PointsHistory> GetByVisitorParkInfoId(int visitorParkInfoId)
    {
        return Query()
            .Where(ph => ph.VisitorParkInformationId == visitorParkInfoId)
            .OrderByDescending(ph => ph.TransactionDate)
            .ToList();
    }

    public List<PointsHistory> GetByDateRange(int visitorParkInfoId, DateTime startDate, DateTime endDate)
    {
        return Query()
            .Where(ph => ph.VisitorParkInformationId == visitorParkInfoId &&
                         ph.TransactionDate >= startDate &&
                         ph.TransactionDate <= endDate)
            .OrderByDescending(ph => ph.TransactionDate)
            .ToList();
    }

    public int GetTotalPointsByVisitor(int visitorParkInfoId)
    {
        return Query()
            .Where(ph => ph.VisitorParkInformationId == visitorParkInfoId)
            .Sum(ph => ph.Points);
    }
}
