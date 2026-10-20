using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetAttractionVisitsResponseTest
{
    [TestMethod]
    public void CreateAttractionVisitsResponse_IsNotNull()
    {
        Assert.IsNotNull(new GetAttractionVisitsResponse());
    }

    [TestMethod]
    public void SetAttractionId_GetCorrectly()
    {
        var response = new GetAttractionVisitsResponse();
        response.AttractionId = 1;
        Assert.AreEqual(response.AttractionId, 1);
    }

    [TestMethod]
    public void SetVisitCount_GetCorrectly()
    {
        var response = new GetAttractionVisitsResponse();
        response.VisitsCount = 1;
        Assert.AreEqual(response.VisitsCount, 1);
    }

    [TestMethod]
    public void SetStartDate_GetCorrectly()
    {
        var response = new GetAttractionVisitsResponse();
        response.StartDate = "10/10/2010";
        Assert.AreEqual(response.StartDate, "10/10/2010");
    }

    [TestMethod]
    public void SetEndDate_GetCorrectly()
    {
        var response = new GetAttractionVisitsResponse();
        response.EndDate = "10/10/2010";
        Assert.AreEqual(response.EndDate, "10/10/2010");
    }
}
