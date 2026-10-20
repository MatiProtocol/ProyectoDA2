using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class SetStrategyRequestTest
{
    private SetStrategyRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new SetStrategyRequest
        {
            StrategyName = "AttractionBased"
        };
    }

    [TestMethod]
    public void SetStrategyRequest_WithValidData_CreatesInstance()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void SetStrategyRequest_SetsStrategyName()
    {
        Assert.AreEqual("AttractionBased", _request.StrategyName);
    }

    [TestMethod]
    public void SetStrategyRequest_WithComboStrategy_SetsCorrectValue()
    {
        var request = new SetStrategyRequest
        {
            StrategyName = "Combo"
        };

        Assert.AreEqual("Combo", request.StrategyName);
    }

    [TestMethod]
    public void SetStrategyRequest_WithEventStrategy_SetsCorrectValue()
    {
        var request = new SetStrategyRequest
        {
            StrategyName = "Event"
        };

        Assert.AreEqual("Event", request.StrategyName);
    }

    [TestMethod]
    public void SetStrategyRequest_CanChangeStrategyName()
    {
        _request.StrategyName = "NewStrategy";

        Assert.AreEqual("NewStrategy", _request.StrategyName);
    }
}
