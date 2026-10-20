using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class AddPointsRequestTest
{
    private AddPointsRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new AddPointsRequest
        {
            VisitorId = 1,
            Points = 50
        };
    }

    [TestMethod]
    public void AddPointsRequest_WithValidData_CreatesInstance()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void AddPointsRequest_SetsVisitorId()
    {
        Assert.AreEqual(1, _request.VisitorId);
    }

    [TestMethod]
    public void AddPointsRequest_SetsPoints()
    {
        Assert.AreEqual(50, _request.Points);
    }

    [TestMethod]
    public void AddPointsRequest_WithDifferentVisitorId_SetsCorrectValue()
    {
        var request = new AddPointsRequest
        {
            VisitorId = 10,
            Points = 100
        };

        Assert.AreEqual(10, request.VisitorId);
    }

    [TestMethod]
    public void AddPointsRequest_WithDifferentPoints_SetsCorrectValue()
    {
        var request = new AddPointsRequest
        {
            VisitorId = 1,
            Points = 200
        };

        Assert.AreEqual(200, request.Points);
    }

    [TestMethod]
    public void AddPointsRequest_WithNegativePoints_SetsNegativeValue()
    {
        var request = new AddPointsRequest
        {
            VisitorId = 1,
            Points = -50
        };

        Assert.AreEqual(-50, request.Points);
    }
}
