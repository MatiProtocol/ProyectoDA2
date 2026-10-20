using Abstraction;
using DataAccess;
using Domain;

namespace Tests.RepositoriesTest;

[TestClass]
public class RewardRedemptionRepositoryTest
{
    private SqlContext _context = null!;
    private IRewardRedemptionRepository _repository = null!;
    private VisitorParkInformation _visitorParkInfo = null!;
    private Reward _reward = null!;
    private RewardRedemption _redemption = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _visitorParkInfo = new VisitorParkInformation
        {
            Id = 1,
            Points = 5000,
            VisitorMembership = "Premium"
        };

        _reward = new Reward
        {
            Id = 1,
            Name = "Free Ticket",
            Description = "One free entrance",
            PointsCost = 1000,
            AvailableQuantity = 10,
            Active = true
        };

        _context.VisitorParkInformations.Add(_visitorParkInfo);
        _context.Rewards.Add(_reward);
        _context.SaveChanges();

        _redemption = new RewardRedemption
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            RewardId = _reward.Id,
            RedemptionDate = DateTime.Now,
            PointsSpent = 1000,
            Status = RedemptionStatus.Completed
        };

        _context.RewardRedemptions.Add(_redemption);
        _context.SaveChanges();

        _repository = new RewardRedemptionRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [TestMethod]
    public void CreateRewardRedemptionRepository_IsNotNull()
    {
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public void GetByVisitorParkInfoId_ReturnsRedemptions()
    {
        var result = _repository.GetByVisitorParkInfoId(_visitorParkInfo.Id);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetByVisitorParkInfoId_WithNoRedemptions_ReturnsEmptyList()
    {
        var result = _repository.GetByVisitorParkInfoId(999);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetByVisitorParkInfoId_OrdersByDateDescending()
    {
        var olderRedemption = new RewardRedemption
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            RewardId = _reward.Id,
            RedemptionDate = DateTime.Now.AddDays(-5),
            PointsSpent = 500,
            Status = RedemptionStatus.Completed
        };
        _context.RewardRedemptions.Add(olderRedemption);
        _context.SaveChanges();

        var result = _repository.GetByVisitorParkInfoId(_visitorParkInfo.Id);

        Assert.IsTrue(result[0].RedemptionDate > result[1].RedemptionDate);
    }

    [TestMethod]
    public void GetByDateRange_ReturnsRedemptionsInRange()
    {
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now.AddDays(1);

        var result = _repository.GetByDateRange(startDate, endDate);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetByDateRange_WithNoRedemptionsInRange_ReturnsEmptyList()
    {
        var startDate = DateTime.Now.AddDays(-10);
        var endDate = DateTime.Now.AddDays(-5);

        var result = _repository.GetByDateRange(startDate, endDate);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetByDateRange_WithMultipleRedemptions_ReturnsOnlyInRange()
    {
        var oldRedemption = new RewardRedemption
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            RewardId = _reward.Id,
            RedemptionDate = DateTime.Now.AddDays(-20),
            PointsSpent = 500,
            Status = RedemptionStatus.Completed
        };
        _context.RewardRedemptions.Add(oldRedemption);
        _context.SaveChanges();

        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now.AddDays(1);

        var result = _repository.GetByDateRange(startDate, endDate);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetWithDetails_WithNonExistingId_ReturnsNull()
    {
        var result = _repository.GetWithDetails(999);

        Assert.IsNull(result);
    }
}
