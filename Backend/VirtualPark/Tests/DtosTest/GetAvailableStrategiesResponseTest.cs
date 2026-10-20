using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetAvailableStrategiesResponseTest
{
    [TestMethod]
    public void GetAvailableStrategiesResponse_WithStrategiesList_CreatesInstance()
    {
        var strategies = new List<string> { "Strategy1", "Strategy2" };

        var response = new GetAvailableStrategiesResponse(strategies);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_SetsStrategies()
    {
        var strategies = new List<string> { "AttractionBased", "Combo", "Event" };

        var response = new GetAvailableStrategiesResponse(strategies);

        Assert.AreEqual(3, response.Strategies.Count);
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_WithEmptyList_SetsEmptyStrategies()
    {
        var strategies = new List<string>();

        var response = new GetAvailableStrategiesResponse(strategies);

        Assert.AreEqual(0, response.Strategies.Count);
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_ContainsAttractionBased()
    {
        var strategies = new List<string> { "AttractionBased", "Combo", "Event" };

        var response = new GetAvailableStrategiesResponse(strategies);

        Assert.IsTrue(response.Strategies.Contains("AttractionBased"));
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_ContainsCombo()
    {
        var strategies = new List<string> { "AttractionBased", "Combo", "Event" };

        var response = new GetAvailableStrategiesResponse(strategies);

        Assert.IsTrue(response.Strategies.Contains("Combo"));
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_ContainsEvent()
    {
        var strategies = new List<string> { "AttractionBased", "Combo", "Event" };

        var response = new GetAvailableStrategiesResponse(strategies);

        Assert.IsTrue(response.Strategies.Contains("Event"));
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_SetStrategies_UpdatesStrategies()
    {
        var strategies = new List<string> { "Strategy1", "Strategy2" };
        var response = new GetAvailableStrategiesResponse(strategies);

        response.Strategies = ["NewStrategy1", "NewStrategy2", "NewStrategy3"];

        Assert.AreEqual(3, response.Strategies.Count);
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_SetStrategies_ContainsNewStrategies()
    {
        var strategies = new List<string> { "Strategy1" };
        var response = new GetAvailableStrategiesResponse(strategies);

        response.Strategies = ["AttractionBased", "Combo"];

        Assert.IsTrue(response.Strategies.Contains("AttractionBased"));
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_SetStrategiesToEmpty_UpdatesToEmpty()
    {
        var strategies = new List<string> { "Strategy1", "Strategy2" };
        var response = new GetAvailableStrategiesResponse(strategies);

        response.Strategies = [];

        Assert.AreEqual(0, response.Strategies.Count);
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_SetStrategiesToNull_UpdatesToNull()
    {
        var strategies = new List<string> { "Strategy1", "Strategy2" };
        var response = new GetAvailableStrategiesResponse(strategies);

        response.Strategies = null!;

        Assert.IsNull(response.Strategies);
    }
}
