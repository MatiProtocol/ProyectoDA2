using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetActiveStrategyResponseTest
{
    [TestMethod]
    public void GetActiveStrategyResponse_WithStrategyName_CreatesInstance()
    {
        var response = new GetActiveStrategyResponse("AttractionBased");

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_SetsStrategyName()
    {
        var response = new GetActiveStrategyResponse("AttractionBased");

        Assert.AreEqual("AttractionBased", response.StrategyName);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_WithComboStrategy_SetsCorrectName()
    {
        var response = new GetActiveStrategyResponse("Combo");

        Assert.AreEqual("Combo", response.StrategyName);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_WithEventStrategy_SetsCorrectName()
    {
        var response = new GetActiveStrategyResponse("Event");

        Assert.AreEqual("Event", response.StrategyName);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_SetStrategyName_UpdatesStrategyName()
    {
        var response = new GetActiveStrategyResponse("AttractionBased");

        response.StrategyName = "Combo";

        Assert.AreEqual("Combo", response.StrategyName);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_SetStrategyNameToEvent_UpdatesToEvent()
    {
        var response = new GetActiveStrategyResponse("AttractionBased");

        response.StrategyName = "Event";

        Assert.AreEqual("Event", response.StrategyName);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_SetStrategyNameToEmpty_UpdatesToEmpty()
    {
        var response = new GetActiveStrategyResponse("AttractionBased");

        response.StrategyName = string.Empty;

        Assert.AreEqual(string.Empty, response.StrategyName);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_SetStrategyNameToNull_UpdatesToNull()
    {
        var response = new GetActiveStrategyResponse("AttractionBased");

        response.StrategyName = null!;

        Assert.IsNull(response.StrategyName);
    }
}
