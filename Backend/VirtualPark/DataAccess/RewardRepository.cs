using Abstraction;
using Domain;

namespace DataAccess;

public class RewardRepository(SqlContext context) : Repository<Reward>(context), IRewardRepository
{
    public List<Reward> GetActiveRewards()
    {
        return Query()
            .Where(r => r.Active)
            .ToList();
    }

    public List<Reward> GetRewardsByMembershipLevel(string? membershipLevel)
    {
        var query = Query().Where(r => r.Active);
        if(!string.IsNullOrEmpty(membershipLevel))
        {
            query = query.Where(r => r.RequiredMembershipLevel == null ||
                                     r.RequiredMembershipLevel == membershipLevel);
        }

        return query.ToList();
    }
}
