using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class ProcessAccessResponseTest
{
    [TestMethod]
    public void ProcessAccessResponse_WithValidData_CreatesInstance()
    {
        var response = new ProcessAccessResponse(1, 50, 150);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetsVisitorId()
    {
        var response = new ProcessAccessResponse(3, 80, 280);

        Assert.AreEqual(3, response.VisitorId);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetsPointsAwarded()
    {
        var response = new ProcessAccessResponse(3, 80, 280);

        Assert.AreEqual(80, response.PointsAwarded);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetsTotalPoints()
    {
        var response = new ProcessAccessResponse(3, 80, 280);

        Assert.AreEqual(280, response.TotalPoints);
    }

    [TestMethod]
    public void ProcessAccessResponse_WithZeroPointsAwarded_SetsZero()
    {
        var response = new ProcessAccessResponse(1, 0, 100);

        Assert.AreEqual(0, response.PointsAwarded);
    }

    [TestMethod]
    public void ProcessAccessResponse_TotalPointsEqualsAwardedPlusExisting()
    {
        var response = new ProcessAccessResponse(2, 50, 50);

        Assert.AreEqual(50, response.TotalPoints);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetVisitorId_UpdatesVisitorId()
    {
        var response = new ProcessAccessResponse(1, 50, 150);

        response.VisitorId = 10;

        Assert.AreEqual(10, response.VisitorId);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetPointsAwarded_UpdatesPointsAwarded()
    {
        var response = new ProcessAccessResponse(1, 50, 150);

        response.PointsAwarded = 100;

        Assert.AreEqual(100, response.PointsAwarded);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetTotalPoints_UpdatesTotalPoints()
    {
        var response = new ProcessAccessResponse(1, 50, 150);

        response.TotalPoints = 500;

        Assert.AreEqual(500, response.TotalPoints);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetVisitorIdToZero_UpdatesToZero()
    {
        var response = new ProcessAccessResponse(1, 50, 150);

        response.VisitorId = 0;

        Assert.AreEqual(0, response.VisitorId);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetPointsAwardedToNegative_UpdatesToNegative()
    {
        var response = new ProcessAccessResponse(1, 50, 150);

        response.PointsAwarded = -50;

        Assert.AreEqual(-50, response.PointsAwarded);
    }

    [TestMethod]
    public void ProcessAccessResponse_SetTotalPointsToZero_UpdatesToZero()
    {
        var response = new ProcessAccessResponse(1, 50, 150);

        response.TotalPoints = 0;

        Assert.AreEqual(0, response.TotalPoints);
    }
}
