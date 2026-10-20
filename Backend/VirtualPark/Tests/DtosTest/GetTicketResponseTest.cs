using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetTicketResponseTest
{
    private GetTicketResponse _response = null!;
    private Ticket _ticket = null!;

    [TestInitialize]
    public void Initialize()
    {
        _ticket = new Ticket
        {
            Id = 0,
            Consumed = true,
            Date = DateTime.Now,
            QR = Guid.Empty,
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 0
        };

        _response = new GetTicketResponse(_ticket);
    }

    [TestMethod]
    public void GetTicketResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _response.Id = 0;
        Assert.AreEqual(_response.Id, 0);
    }

    [TestMethod]
    public void SetQr_GetCorrectly()
    {
        var guid = Guid.NewGuid();
        _response.QR = guid;
        Assert.AreEqual(_response.QR, guid);
    }

    [TestMethod]
    public void SetVisitorParkInformation_GetCorrectly()
    {
        _response.VisitorParkInformationId = 1;
        Assert.AreEqual(_response.VisitorParkInformationId, 1);
    }

    [TestMethod]
    public void SetConsumed_GetCorrectly()
    {
        _response.Consumed = false;
        Assert.IsFalse(_response.Consumed);
    }

    [TestMethod]
    public void SetTicketType_GetCorrectly()
    {
        _response.TicketType = nameof(TicketType.SpecialEvent);
        Assert.AreEqual(_response.TicketType, nameof(TicketType.SpecialEvent));
    }

    [TestMethod]
    public void SetDate_GetCorrectly()
    {
        _response.Date = DateTime.Now.Date;
        Assert.AreEqual(_response.Date.Date, DateTime.Now.Date);
    }

    [TestMethod]
    public void SetSpecialEventId()
    {
        _response.SpecialEventId = 1;
        Assert.AreEqual(1, _response.SpecialEventId);
    }
}
