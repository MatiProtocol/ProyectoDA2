using Domain;

namespace Tests.DomainTest;

[TestClass]
public class RewardRedemptionTest
{
    private RewardRedemption _redemption = null!;
    private DateTime _testDate;

    [TestInitialize]
    public void Initialize()
    {
        _testDate = new DateTime(2025, 10, 15, 14, 30, 0);
        _redemption = new RewardRedemption
        {
            Id = 1,
            VisitorParkInformationId = 10,
            RewardId = 5,
            RedemptionDate = _testDate,
            PointsSpent = 500,
            Status = RedemptionStatus.Completed
        };
    }

    [TestMethod]
    public void CreateRewardRedemption_IsNotNull()
    {
        Assert.IsNotNull(_redemption);
    }

    [TestMethod]
    public void CreateRewardRedemption_WithEmptyConstructor_IsNotNull()
    {
        var redemption = new RewardRedemption();
        Assert.IsNotNull(redemption);
    }

    [TestMethod]
    public void CreateRewardRedemption_WithEmptyConstructor_StatusIsPending()
    {
        var redemption = new RewardRedemption();
        Assert.AreEqual(RedemptionStatus.Pending, redemption.Status);
    }

    [TestMethod]
    public void CreateRewardRedemption_WithEmptyConstructor_RedemptionDateIsSet()
    {
        var before = DateTime.Now;
        var redemption = new RewardRedemption();
        var after = DateTime.Now;
        Assert.IsTrue(redemption.RedemptionDate >= before && redemption.RedemptionDate <= after);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _redemption.Id = 99;
        Assert.AreEqual(99, _redemption.Id);
    }

    [TestMethod]
    public void SetVisitorParkInformationId_GetCorrectly()
    {
        _redemption.VisitorParkInformationId = 25;
        Assert.AreEqual(25, _redemption.VisitorParkInformationId);
    }

    [TestMethod]
    public void SetRewardId_GetCorrectly()
    {
        _redemption.RewardId = 15;
        Assert.AreEqual(15, _redemption.RewardId);
    }

    [TestMethod]
    public void SetRedemptionDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 12, 25);
        _redemption.RedemptionDate = newDate;
        Assert.AreEqual(newDate, _redemption.RedemptionDate);
    }

    [TestMethod]
    public void SetPointsSpent_GetCorrectly()
    {
        _redemption.PointsSpent = 1500;
        Assert.AreEqual(1500, _redemption.PointsSpent);
    }

    [TestMethod]
    public void SetStatus_ToPending_GetCorrectly()
    {
        _redemption.Status = RedemptionStatus.Pending;
        Assert.AreEqual(RedemptionStatus.Pending, _redemption.Status);
    }

    [TestMethod]
    public void SetStatus_ToCompleted_GetCorrectly()
    {
        _redemption.Status = RedemptionStatus.Completed;
        Assert.AreEqual(RedemptionStatus.Completed, _redemption.Status);
    }

    [TestMethod]
    public void SetStatus_ToCancelled_GetCorrectly()
    {
        _redemption.Status = RedemptionStatus.Cancelled;
        Assert.AreEqual(RedemptionStatus.Cancelled, _redemption.Status);
    }

    [TestMethod]
    public void SetReward_GetCorrectly()
    {
        var reward = new Reward { Id = 10, Name = "Test Reward" };
        _redemption.Reward = reward;
        Assert.AreEqual(10, _redemption.Reward.Id);
    }

    [TestMethod]
    public void SetReward_ToNull_GetNull()
    {
        _redemption.Reward = null;
        Assert.IsNull(_redemption.Reward);
    }

    [TestMethod]
    public void SetVisitorParkInformation_GetCorrectly()
    {
        var visitorParkInfo = new VisitorParkInformation { Id = 5 };
        _redemption.VisitorParkInformation = visitorParkInfo;
        Assert.AreEqual(5, _redemption.VisitorParkInformation.Id);
    }

    [TestMethod]
    public void SetVisitorParkInformation_ToNull_GetNull()
    {
        _redemption.VisitorParkInformation = null;
        Assert.IsNull(_redemption.VisitorParkInformation);
    }
}
