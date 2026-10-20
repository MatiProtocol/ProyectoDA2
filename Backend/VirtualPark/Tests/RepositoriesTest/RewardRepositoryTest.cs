using Abstraction;
using DataAccess;
using Domain;

namespace Tests.RepositoriesTest;

[TestClass]
public class RewardRepositoryTest
{
    private SqlContext _context = null!;
    private IRewardRepository _repository = null!;
    private Reward _activeReward = null!;
    private Reward _inactiveReward = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _activeReward = new Reward
        {
            Name = "Free Ticket",
            Description = "One free entrance",
            PointsCost = 1000,
            AvailableQuantity = 10,
            RequiredMembershipLevel = "Premium",
            Active = true
        };

        _inactiveReward = new Reward
        {
            Name = "Discount Coupon",
            Description = "50% off",
            PointsCost = 500,
            AvailableQuantity = 5,
            RequiredMembershipLevel = "Standard",
            Active = false
        };

        _context.Rewards.AddRange(_activeReward);
        _context.Rewards.AddRange(_inactiveReward);
        _context.SaveChanges();

        _repository = new RewardRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [TestMethod]
    public void CreateRewardRepository_IsNotNull()
    {
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public void GetActiveRewards_ReturnsOnlyActiveRewards()
    {
        var result = _repository.GetActiveRewards();

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetActiveRewards_WithNoActiveRewards_ReturnsEmptyList()
    {
        _activeReward.Active = false;
        _context.SaveChanges();

        var result = _repository.GetActiveRewards();

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetRewardsByMembershipLevel_WithNullMembership_ReturnsAllActiveRewards()
    {
        var result = _repository.GetRewardsByMembershipLevel(null);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetRewardsByMembershipLevel_WithSpecificLevel_ReturnsMatchingRewards()
    {
        var standardReward = new Reward
        {
            Name = "Standard Reward",
            Description = "For standard members",
            PointsCost = 200,
            AvailableQuantity = 20,
            RequiredMembershipLevel = "Standard",
            Active = true
        };
        _context.Rewards.Add(standardReward);
        _context.SaveChanges();

        var result = _repository.GetRewardsByMembershipLevel("Standard");

        Assert.AreEqual("Standard Reward", result[0].Name);
    }

    [TestMethod]
    public void GetRewardsByMembershipLevel_IncludesRewardsWithNoRequirement()
    {
        var noRequirementReward = new Reward
        {
            Name = "General Reward",
            Description = "For everyone",
            PointsCost = 300,
            AvailableQuantity = 15,
            RequiredMembershipLevel = null,
            Active = true
        };
        _context.Rewards.Add(noRequirementReward);
        _context.SaveChanges();

        var result = _repository.GetRewardsByMembershipLevel("Premium");

        Assert.AreEqual(2, result.Count);
    }
}
