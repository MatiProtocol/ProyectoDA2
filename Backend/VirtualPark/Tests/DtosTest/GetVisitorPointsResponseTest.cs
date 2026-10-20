using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetVisitorPointsResponseTest
{
    [TestMethod]
    public void GetVisitorPointsResponse_WithValidData_CreatesInstance()
    {
        var response = new GetVisitorPointsResponse(1, 100);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void GetVisitorPointsResponse_SetsVisitorId()
    {
        var response = new GetVisitorPointsResponse(5, 200);

        Assert.AreEqual(5, response.VisitorId);
    }

    [TestMethod]
    public void GetVisitorPointsResponse_SetsTotalPoints()
    {
        var response = new GetVisitorPointsResponse(5, 200);

        Assert.AreEqual(200, response.TotalPoints);
    }

    [TestMethod]
    public void GetVisitorPointsResponse_WithZeroPoints_SetsZeroPoints()
    {
        var response = new GetVisitorPointsResponse(3, 0);

        Assert.AreEqual(0, response.TotalPoints);
    }

    [TestMethod]
    public void GetVisitorPointsResponse_WithLargePoints_SetsCorrectValue()
    {
        var response = new GetVisitorPointsResponse(10, 99999);

        Assert.AreEqual(99999, response.TotalPoints);
    }
}
