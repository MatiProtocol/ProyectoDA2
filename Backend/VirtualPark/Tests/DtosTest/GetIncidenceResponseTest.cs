using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetIncidenceResponseTest
{
    private Incidence _incidence = null!;
    private GetIncidenceResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _incidence = new Incidence
        {
            Id = 1,
            Description = "Test incidence",
            AttractionId = 5,
            Active = true
        };
        _response = new GetIncidenceResponse(_incidence);
    }

    [TestMethod]
    public void GetIncidenceResponse_WithValidIncidence_CreatesInstance()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetsId()
    {
        Assert.AreEqual(1, _response.Id);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetsDescription()
    {
        Assert.AreEqual("Test incidence", _response.Description);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetsAttractionId()
    {
        Assert.AreEqual(5, _response.AttractionId);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetsActive()
    {
        Assert.IsTrue(_response.Active);
    }

    [TestMethod]
    public void GetIncidenceResponse_WithInactiveIncidence_SetsActiveFalse()
    {
        var incidence = new Incidence
        {
            Id = 2,
            Description = "Inactive",
            AttractionId = 3,
            Active = false
        };

        var response = new GetIncidenceResponse(incidence);

        Assert.IsFalse(response.Active);
    }

    [TestMethod]
    public void GetIncidenceResponse_WithoutAttraction_SetsAttractionNameEmpty()
    {
        Assert.AreEqual(string.Empty, _response.AttractionName);
    }

    [TestMethod]
    public void GetIncidenceResponse_WithAttraction_SetsAttractionName()
    {
        var incidence = new Incidence
        {
            Id = 3,
            Description = "Test",
            AttractionId = 1,
            Active = true,
            AttractionName = "Roller Coaster"
        };

        var response = new GetIncidenceResponse(incidence);

        Assert.AreEqual("Roller Coaster", response.AttractionName);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetId_UpdatesId()
    {
        _response.Id = 10;

        Assert.AreEqual(10, _response.Id);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetDescription_UpdatesDescription()
    {
        _response.Description = "Updated description";

        Assert.AreEqual("Updated description", _response.Description);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetAttractionId_UpdatesAttractionId()
    {
        _response.AttractionId = 99;

        Assert.AreEqual(99, _response.AttractionId);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetActive_UpdatesActive()
    {
        _response.Active = false;

        Assert.IsFalse(_response.Active);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetAttractionName_UpdatesAttractionName()
    {
        _response.AttractionName = "New Attraction Name";

        Assert.AreEqual("New Attraction Name", _response.AttractionName);
    }

    [TestMethod]
    public void GetIncidenceResponse_SetAttractionNameToNull_UpdatesToNull()
    {
        _response.AttractionName = null!;

        Assert.IsNull(_response.AttractionName);
    }

    [TestMethod]
    public void SetIsMaintenance_GetCorrectly()
    {
        _response.IsMaintenance = true;
        Assert.AreEqual(_response.IsMaintenance, true);
    }

    [TestMethod]
    public void SetStartDate_GetCorrectly()
    {
        _response.StartDate = DateTime.Today;
        Assert.AreEqual(_response.StartDate, DateTime.Today);
    }

    [TestMethod]
    public void SetStartTime_GetCorrectly()
    {
        var time = DateTime.Today.TimeOfDay;
        _response.StartTime = time;
        Assert.AreEqual(_response.StartTime, time);
    }

    [TestMethod]
    public void SetDuration_GetCorrectly()
    {
        _response.Duration = 10;
        Assert.AreEqual(_response.Duration, 10);
    }
}
