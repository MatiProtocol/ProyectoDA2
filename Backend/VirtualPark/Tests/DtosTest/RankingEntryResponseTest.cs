using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class RankingEntryResponseTest
{
    [TestMethod]
    public void RankingEntryResponse_WithValidData_CreatesInstance()
    {
        var response = new RankingEntryResponse("Test User", 150, 1);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void RankingEntryResponse_SetsVisitorName()
    {
        var response = new RankingEntryResponse("John Doe", 200, 1);

        Assert.AreEqual("John Doe", response.VisitorName);
    }

    [TestMethod]
    public void RankingEntryResponse_SetsPoints()
    {
        var response = new RankingEntryResponse("John Doe", 200, 1);

        Assert.AreEqual(200, response.Points);
    }

    [TestMethod]
    public void RankingEntryResponse_SetsPosition()
    {
        var response = new RankingEntryResponse("John Doe", 200, 3);

        Assert.AreEqual(3, response.Position);
    }

    [TestMethod]
    public void RankingEntryResponse_WithFirstPosition_SetsPositionOne()
    {
        var response = new RankingEntryResponse("Winner", 500, 1);

        Assert.AreEqual(1, response.Position);
    }

    [TestMethod]
    public void RankingEntryResponse_WithLastPosition_SetsCorrectPosition()
    {
        var response = new RankingEntryResponse("Last Place", 10, 10);

        Assert.AreEqual(10, response.Position);
    }

    [TestMethod]
    public void RankingEntryResponse_SetVisitorName_UpdatesVisitorName()
    {
        var response = new RankingEntryResponse("Original Name", 100, 1);

        response.VisitorName = "Updated Name";

        Assert.AreEqual("Updated Name", response.VisitorName);
    }

    [TestMethod]
    public void RankingEntryResponse_SetPoints_UpdatesPoints()
    {
        var response = new RankingEntryResponse("Test User", 100, 1);

        response.Points = 500;

        Assert.AreEqual(500, response.Points);
    }

    [TestMethod]
    public void RankingEntryResponse_SetPosition_UpdatesPosition()
    {
        var response = new RankingEntryResponse("Test User", 100, 1);

        response.Position = 5;

        Assert.AreEqual(5, response.Position);
    }

    [TestMethod]
    public void RankingEntryResponse_SetPointsToZero_UpdatesToZero()
    {
        var response = new RankingEntryResponse("Test User", 100, 1);

        response.Points = 0;

        Assert.AreEqual(0, response.Points);
    }

    [TestMethod]
    public void RankingEntryResponse_SetPointsToNegative_UpdatesToNegative()
    {
        var response = new RankingEntryResponse("Test User", 100, 1);

        response.Points = -50;

        Assert.AreEqual(-50, response.Points);
    }

    [TestMethod]
    public void RankingEntryResponse_SetPositionToZero_UpdatesToZero()
    {
        var response = new RankingEntryResponse("Test User", 100, 1);

        response.Position = 0;

        Assert.AreEqual(0, response.Position);
    }
}
