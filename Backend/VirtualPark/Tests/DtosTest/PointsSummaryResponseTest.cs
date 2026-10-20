using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class PointsSummaryResponseTest
{
    private PointsSummaryResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _response = new PointsSummaryResponse();
    }

    [TestMethod]
    public void PointsSummaryResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void SetVisitorId_GetCorrectly()
    {
        _response.VisitorId = 5;
        Assert.AreEqual(5, _response.VisitorId);
    }

    [TestMethod]
    public void SetTotalPointsEarned_GetCorrectly()
    {
        _response.TotalPointsEarned = 1000;
        Assert.AreEqual(1000, _response.TotalPointsEarned);
    }

    [TestMethod]
    public void SetTotalPointsSpent_GetCorrectly()
    {
        _response.TotalPointsSpent = 500;
        Assert.AreEqual(500, _response.TotalPointsSpent);
    }

    [TestMethod]
    public void SetCurrentBalance_GetCorrectly()
    {
        _response.CurrentBalance = 1500;
        Assert.AreEqual(1500, _response.CurrentBalance);
    }

    [TestMethod]
    public void SetTodayPoints_GetCorrectly()
    {
        _response.TodayPoints = 100;
        Assert.AreEqual(100, _response.TodayPoints);
    }

    [TestMethod]
    public void SetRedemptionsToday_GetCorrectly()
    {
        _response.RedemptionsToday = 3;
        Assert.AreEqual(3, _response.RedemptionsToday);
    }

    [TestMethod]
    public void SetVisitorIdToZero_GetCorrectly()
    {
        _response.VisitorId = 0;
        Assert.AreEqual(0, _response.VisitorId);
    }

    [TestMethod]
    public void SetTotalPointsEarnedToZero_GetCorrectly()
    {
        _response.TotalPointsEarned = 0;
        Assert.AreEqual(0, _response.TotalPointsEarned);
    }

    [TestMethod]
    public void SetTotalPointsSpentToZero_GetCorrectly()
    {
        _response.TotalPointsSpent = 0;
        Assert.AreEqual(0, _response.TotalPointsSpent);
    }

    [TestMethod]
    public void SetCurrentBalanceToZero_GetCorrectly()
    {
        _response.CurrentBalance = 0;
        Assert.AreEqual(0, _response.CurrentBalance);
    }

    [TestMethod]
    public void SetTodayPointsToZero_GetCorrectly()
    {
        _response.TodayPoints = 0;
        Assert.AreEqual(0, _response.TodayPoints);
    }

    [TestMethod]
    public void SetRedemptionsTodayToZero_GetCorrectly()
    {
        _response.RedemptionsToday = 0;
        Assert.AreEqual(0, _response.RedemptionsToday);
    }

    [TestMethod]
    public void SetVisitorIdToNegative_GetCorrectly()
    {
        _response.VisitorId = -1;
        Assert.AreEqual(-1, _response.VisitorId);
    }

    [TestMethod]
    public void SetTotalPointsEarnedToNegative_GetCorrectly()
    {
        _response.TotalPointsEarned = -100;
        Assert.AreEqual(-100, _response.TotalPointsEarned);
    }

    [TestMethod]
    public void SetTotalPointsSpentToNegative_GetCorrectly()
    {
        _response.TotalPointsSpent = -50;
        Assert.AreEqual(-50, _response.TotalPointsSpent);
    }

    [TestMethod]
    public void SetCurrentBalanceToNegative_GetCorrectly()
    {
        _response.CurrentBalance = -200;
        Assert.AreEqual(-200, _response.CurrentBalance);
    }

    [TestMethod]
    public void SetTodayPointsToNegative_GetCorrectly()
    {
        _response.TodayPoints = -10;
        Assert.AreEqual(-10, _response.TodayPoints);
    }

    [TestMethod]
    public void SetRedemptionsTodayToNegative_GetCorrectly()
    {
        _response.RedemptionsToday = -2;
        Assert.AreEqual(-2, _response.RedemptionsToday);
    }

    [TestMethod]
    public void SetVisitorIdToMaxValue_GetCorrectly()
    {
        _response.VisitorId = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _response.VisitorId);
    }

    [TestMethod]
    public void SetTotalPointsEarnedToMaxValue_GetCorrectly()
    {
        _response.TotalPointsEarned = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _response.TotalPointsEarned);
    }

    [TestMethod]
    public void SetTotalPointsSpentToMaxValue_GetCorrectly()
    {
        _response.TotalPointsSpent = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _response.TotalPointsSpent);
    }

    [TestMethod]
    public void SetCurrentBalanceToMaxValue_GetCorrectly()
    {
        _response.CurrentBalance = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _response.CurrentBalance);
    }

    [TestMethod]
    public void SetTodayPointsToMaxValue_GetCorrectly()
    {
        _response.TodayPoints = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _response.TodayPoints);
    }

    [TestMethod]
    public void SetRedemptionsTodayToMaxValue_GetCorrectly()
    {
        _response.RedemptionsToday = int.MaxValue;
        Assert.AreEqual(int.MaxValue, _response.RedemptionsToday);
    }
}
