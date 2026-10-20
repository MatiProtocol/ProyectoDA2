using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IRewardRedemptionRepository : INonUserRepository<RewardRedemption>
{
    List<RewardRedemption> GetByVisitorParkInfoId(int visitorParkInfoId);
    List<RewardRedemption> GetByDateRange(DateTime startDate, DateTime endDate);
    RewardRedemption? GetWithDetails(int redemptionId);
}
