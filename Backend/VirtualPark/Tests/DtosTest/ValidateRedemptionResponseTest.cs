using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class ValidateRedemptionResponseTest
{
    private ValidateRedemptionResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _response = new ValidateRedemptionResponse(true, 2000, 1000, null);
    }

    [TestMethod]
    public void ValidateRedemptionResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsCanRedeemTrueCorrectly()
    {
        Assert.IsTrue(_response.CanRedeem);
    }

    [TestMethod]
    public void Constructor_MapsCanRedeemFalseCorrectly()
    {
        var response = new ValidateRedemptionResponse(false, 500, 1000, "Insufficient points");
        Assert.IsFalse(response.CanRedeem);
    }

    [TestMethod]
    public void Constructor_MapsVisitorPointsCorrectly()
    {
        Assert.AreEqual(2000, _response.VisitorPoints);
    }

    [TestMethod]
    public void Constructor_MapsRewardCostCorrectly()
    {
        Assert.AreEqual(1000, _response.RewardCost);
    }

    [TestMethod]
    public void Constructor_MapsReasonIfCannotAsNullCorrectly()
    {
        Assert.IsNull(_response.ReasonIfCannot);
    }

    [TestMethod]
    public void Constructor_MapsReasonIfCannotCorrectly()
    {
        var response = new ValidateRedemptionResponse(false, 500, 1000, "Insufficient points");
        Assert.AreEqual("Insufficient points", response.ReasonIfCannot);
    }

    [TestMethod]
    public void Constructor_WithEmptyReason_MapsCorrectly()
    {
        var response = new ValidateRedemptionResponse(false, 500, 1000, " ");
        Assert.AreEqual(" ", response.ReasonIfCannot);
    }

    [TestMethod]
    public void SetCanRedeem_GetCorrectly()
    {
        _response.CanRedeem = false;
        Assert.IsFalse(_response.CanRedeem);
    }

    [TestMethod]
    public void SetVisitorPoints_GetCorrectly()
    {
        _response.VisitorPoints = 1500;
        Assert.AreEqual(1500, _response.VisitorPoints);
    }

    [TestMethod]
    public void SetRewardCost_GetCorrectly()
    {
        _response.RewardCost = 500;
        Assert.AreEqual(500, _response.RewardCost);
    }

    [TestMethod]
    public void SetReasonIfCannot_GetCorrectly()
    {
        _response.ReasonIfCannot = "Membership requirement not met";
        Assert.AreEqual("Membership requirement not met", _response.ReasonIfCannot);
    }

    [TestMethod]
    public void SetReasonIfCannotToNull_GetCorrectly()
    {
        _response.ReasonIfCannot = null;
        Assert.IsNull(_response.ReasonIfCannot);
    }

    [TestMethod]
    public void SetVisitorPointsToZero_GetCorrectly()
    {
        _response.VisitorPoints = 0;
        Assert.AreEqual(0, _response.VisitorPoints);
    }

    [TestMethod]
    public void SetRewardCostToZero_GetCorrectly()
    {
        _response.RewardCost = 0;
        Assert.AreEqual(0, _response.RewardCost);
    }

    [TestMethod]
    public void SetVisitorPointsToNegative_GetCorrectly()
    {
        _response.VisitorPoints = -100;
        Assert.AreEqual(-100, _response.VisitorPoints);
    }

    [TestMethod]
    public void SetRewardCostToNegative_GetCorrectly()
    {
        _response.RewardCost = -50;
        Assert.AreEqual(-50, _response.RewardCost);
    }

    [TestMethod]
    public void SetReasonIfCannotToEmpty_GetCorrectly()
    {
        _response.ReasonIfCannot = " ";
        Assert.AreEqual(" ", _response.ReasonIfCannot);
    }

    [TestMethod]
    public void SetCanRedeemTrueWithReason_GetCorrectly()
    {
        _response.CanRedeem = true;
        _response.ReasonIfCannot = "Some reason";
        Assert.IsTrue(_response.CanRedeem);
        Assert.AreEqual("Some reason", _response.ReasonIfCannot);
    }

    [TestMethod]
    public void SetCanRedeemFalseWithoutReason_GetCorrectly()
    {
        _response.CanRedeem = false;
        _response.ReasonIfCannot = null;
        Assert.IsFalse(_response.CanRedeem);
        Assert.IsNull(_response.ReasonIfCannot);
    }
}
