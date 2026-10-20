using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class RewardRedemptionResponseTest
{
    private RewardRedemption _redemption = null!;
    private RewardRedemptionResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _redemption = new RewardRedemption
        {
            Id = 1,
            PointsSpent = 1000,
            RedemptionDate = new DateTime(2025, 10, 15, 14, 30, 0),
            Status = RedemptionStatus.Completed
        };
        _response = new RewardRedemptionResponse(_redemption, "Free Ticket", 1500);
    }

    [TestMethod]
    public void RewardRedemptionResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsRedemptionIdCorrectly()
    {
        Assert.AreEqual(1, _response.RedemptionId);
    }

    [TestMethod]
    public void Constructor_MapsRewardNameCorrectly()
    {
        Assert.AreEqual("Free Ticket", _response.RewardName);
    }

    [TestMethod]
    public void Constructor_MapsPointsSpentCorrectly()
    {
        Assert.AreEqual(1000, _response.PointsSpent);
    }

    [TestMethod]
    public void Constructor_MapsRedemptionDateCorrectly()
    {
        Assert.AreEqual(new DateTime(2025, 10, 15, 14, 30, 0), _response.RedemptionDate);
    }

    [TestMethod]
    public void Constructor_MapsStatusCorrectly()
    {
        Assert.AreEqual("Completed", _response.Status);
    }

    [TestMethod]
    public void Constructor_MapsRemainingPointsCorrectly()
    {
        Assert.AreEqual(1500, _response.RemainingPoints);
    }

    [TestMethod]
    public void Constructor_MapsPendingStatusCorrectly()
    {
        _redemption.Status = RedemptionStatus.Pending;
        var response = new RewardRedemptionResponse(_redemption, "Test", 1000);
        Assert.AreEqual("Pending", response.Status);
    }

    [TestMethod]
    public void Constructor_MapsCancelledStatusCorrectly()
    {
        _redemption.Status = RedemptionStatus.Cancelled;
        var response = new RewardRedemptionResponse(_redemption, "Test", 1000);
        Assert.AreEqual("Cancelled", response.Status);
    }

    [TestMethod]
    public void SetRedemptionId_GetCorrectly()
    {
        _response.RedemptionId = 5;
        Assert.AreEqual(5, _response.RedemptionId);
    }

    [TestMethod]
    public void SetRewardName_GetCorrectly()
    {
        _response.RewardName = "Discount Coupon";
        Assert.AreEqual("Discount Coupon", _response.RewardName);
    }

    [TestMethod]
    public void SetPointsSpent_GetCorrectly()
    {
        _response.PointsSpent = 500;
        Assert.AreEqual(500, _response.PointsSpent);
    }

    [TestMethod]
    public void SetRedemptionDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 11, 1, 10, 0, 0);
        _response.RedemptionDate = newDate;
        Assert.AreEqual(newDate, _response.RedemptionDate);
    }

    [TestMethod]
    public void SetStatus_GetCorrectly()
    {
        _response.Status = "Pending";
        Assert.AreEqual("Pending", _response.Status);
    }

    [TestMethod]
    public void SetRemainingPoints_GetCorrectly()
    {
        _response.RemainingPoints = 2000;
        Assert.AreEqual(2000, _response.RemainingPoints);
    }

    [TestMethod]
    public void SetRedemptionIdToZero_GetCorrectly()
    {
        _response.RedemptionId = 0;
        Assert.AreEqual(0, _response.RedemptionId);
    }

    [TestMethod]
    public void SetPointsSpentToZero_GetCorrectly()
    {
        _response.PointsSpent = 0;
        Assert.AreEqual(0, _response.PointsSpent);
    }

    [TestMethod]
    public void SetRemainingPointsToZero_GetCorrectly()
    {
        _response.RemainingPoints = 0;
        Assert.AreEqual(0, _response.RemainingPoints);
    }

    [TestMethod]
    public void SetRewardNameToEmpty_GetCorrectly()
    {
        _response.RewardName = " ";
        Assert.AreEqual(" ", _response.RewardName);
    }

    [TestMethod]
    public void SetStatusToEmpty_GetCorrectly()
    {
        _response.Status = " ";
        Assert.AreEqual(" ", _response.Status);
    }
}
