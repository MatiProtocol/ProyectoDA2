using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class RedeemRewardRequestTest
{
    private RedeemRewardRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new RedeemRewardRequest
        {
            VisitorId = 1,
            RewardId = 1
        };
    }

    [TestMethod]
    public void RedeemRewardRequest_IsNotNull()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void SetVisitorId_GetCorrectly()
    {
        _request.VisitorId = 5;
        Assert.AreEqual(5, _request.VisitorId);
    }

    [TestMethod]
    public void SetRewardId_GetCorrectly()
    {
        _request.RewardId = 10;
        Assert.AreEqual(10, _request.RewardId);
    }

    [TestMethod]
    public void DefaultVisitorId_GetCorrectly()
    {
        Assert.AreEqual(1, _request.VisitorId);
    }

    [TestMethod]
    public void DefaultRewardId_GetCorrectly()
    {
        Assert.AreEqual(1, _request.RewardId);
    }

    [TestMethod]
    public void SetVisitorIdToZero_GetCorrectly()
    {
        _request.VisitorId = 0;
        Assert.AreEqual(0, _request.VisitorId);
    }

    [TestMethod]
    public void SetRewardIdToZero_GetCorrectly()
    {
        _request.RewardId = 0;
        Assert.AreEqual(0, _request.RewardId);
    }

    [TestMethod]
    public void SetVisitorIdToNegative_GetCorrectly()
    {
        _request.VisitorId = -1;
        Assert.AreEqual(-1, _request.VisitorId);
    }

    [TestMethod]
    public void SetRewardIdToNegative_GetCorrectly()
    {
        _request.RewardId = -5;
        Assert.AreEqual(-5, _request.RewardId);
    }

    [TestMethod]
    public void SetVisitorIdToMaxValue_GetCorrectly()
    {
        _request.VisitorId = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _request.VisitorId);
    }

    [TestMethod]
    public void SetRewardIdToMaxValue_GetCorrectly()
    {
        _request.RewardId = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _request.RewardId);
    }
}
