using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class ProcessAttractionAccessRequestTest
{
    private ProcessAttractionAccessRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new ProcessAttractionAccessRequest
        {
            VisitorId = 1,
            AttractionId = 1
        };
    }

    [TestMethod]
    public void ProcessAttractionAccessRequest_WithValidData_CreatesInstance()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void ProcessAttractionAccessRequest_SetsVisitorId()
    {
        Assert.AreEqual(1, _request.VisitorId);
    }

    [TestMethod]
    public void ProcessAttractionAccessRequest_SetsAttractionId()
    {
        _request.AttractionId = 2;
        Assert.AreEqual(2, _request.AttractionId);
    }

    [TestMethod]
    public void SetVisitorNFC_GetCorrectly()
    {
        var nfcGuid = Guid.NewGuid().ToString();
        _request.VisitorNFC = nfcGuid;
        Assert.AreEqual(nfcGuid, _request.VisitorNFC);
    }
}
