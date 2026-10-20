using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IRewardRepository : INonUserRepository<Reward>
{
    List<Reward> GetActiveRewards();
    List<Reward> GetRewardsByMembershipLevel(string? membershipLevel);
}
