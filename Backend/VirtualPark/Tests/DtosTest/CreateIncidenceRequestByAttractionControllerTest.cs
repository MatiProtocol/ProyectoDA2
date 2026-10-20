using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class CreateIncidenceRequestByAttractionControllerTest
{
    [TestMethod]
    public void CreateIncidenceRequest_IsNotNull()
    {
        var createIncidenceRequest = new CreateIncidenceRequestByAttractionController
        {
            Description = "Description"
        };

        Assert.IsNotNull(createIncidenceRequest);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        var createIncidenceRequest = new CreateIncidenceRequestByAttractionController
        {
            Description = "Test"
        };

        Assert.AreEqual(createIncidenceRequest.Description, "Test");
    }

    [TestMethod]
    public void ToEntity_GetIncidence()
    {
        var createIncidenceRequest = new CreateIncidenceRequestByAttractionController
        {
            Description = "Test"
        };

        var incidence = createIncidenceRequest.ToEntity(0);

        Assert.IsNotNull(incidence);
    }
}
