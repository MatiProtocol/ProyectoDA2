using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class AttractionAccessRequestTest
{
    [TestMethod]
    public void AttractionAccessRequest_IsNotNull()
    {
        var attractionAccessRequest = new AttractionAccessRequest();

        Assert.IsNotNull(attractionAccessRequest);
    }

    [TestMethod]
    public void SetTicketQR_GetCorrectly()
    {
        var qr = Guid.Empty;

        var attractionAccessRequest = new AttractionAccessRequest
        {
            TicketQR = qr
        };

        var result = attractionAccessRequest.TicketQR;

        Assert.AreEqual(result, qr);
    }

    [TestMethod]
    public void SetVisitorNFC_GetCorrectly()
    {
        var nfc = Guid.Empty;

        var attractionAccessRequest = new AttractionAccessRequest
        {
            VisitorNFC = nfc
        };

        var result = attractionAccessRequest.VisitorNFC;

        Assert.AreEqual(result, nfc);
    }
}
