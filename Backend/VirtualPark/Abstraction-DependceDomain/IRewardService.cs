using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IRewardService : ICrudService<Reward>
{
    List<Reward> GetAvailableRewardsForVisitor(int visitorId);
    bool ValidateRedemption(int visitorId, int rewardId);
    (bool CanRedeem, string? Reason) ValidateRedemptionWithReason(int visitorId, int rewardId);
    RewardRedemption RedeemReward(int visitorId, int rewardId);
    List<RewardRedemption> GetVisitorRedemptions(int visitorId);
    void CancelRedemption(int redemptionId);
}
