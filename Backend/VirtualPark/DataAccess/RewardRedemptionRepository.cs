using Abstraction;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class RewardRedemptionRepository(SqlContext context) : Repository<RewardRedemption>(context), IRewardRedemptionRepository
{
    public List<RewardRedemption> GetByVisitorParkInfoId(int visitorParkInfoId)
    {
        return Query()
            .Where(rr => rr.VisitorParkInformationId == visitorParkInfoId)
            .OrderByDescending(rr => rr.RedemptionDate)
            .ToList();
    }

    public List<RewardRedemption> GetByDateRange(DateTime startDate, DateTime endDate)
    {
        return Query()
            .Where(rr => rr.RedemptionDate >= startDate &&
                         rr.RedemptionDate <= endDate)
            .ToList();
    }

    public RewardRedemption? GetWithDetails(int redemptionId)
    {
        return Query()
            .Where(rr => rr.Id == redemptionId)
            .Include(rr => rr.Reward)
            .Include(rr => rr.VisitorParkInformation)
            .FirstOrDefault();
    }
}
