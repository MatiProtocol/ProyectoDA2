using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class BuyTicketRequestTest
{
    private BuyTicketRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new BuyTicketRequest
        {
            VisitorId = 0
        };
    }

    [TestMethod]
    public void BuyTicketRequest_WithValidData_CreatesInstance()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void SetEventId_GetCorrectly()
    {
        _request.EventId = 1;
        Assert.AreEqual(1, _request.EventId);
    }

    [TestMethod]
    public void SetVisitorId_GetCorrectly()
    {
        _request.VisitorId = 1;
        Assert.AreEqual(1, _request.VisitorId);
    }
}
