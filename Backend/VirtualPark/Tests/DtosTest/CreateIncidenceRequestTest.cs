using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class CreateIncidenceRequestTest
{
    private CreateIncidenceRequest _request = null!;
    private CreateIncidenceRequest _requestMaintenance = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new CreateIncidenceRequest
        {
            Description = "Test incidence description",
            AttractionId = 1
        };

        _requestMaintenance = new CreateIncidenceRequest
        {
            Description = "Mentenimiento descripto j ja",
            AttractionId = 2,
            IsMaintenance = true,
            StartDate = DateTime.Today,
            StartTime = DateTime.Now.TimeOfDay,
            Duration = 10
        };
    }

    [TestMethod]
    public void CreateIncidenceRequest_WithValidData_CreatesInstance()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void CreateIncidenceRequest_SetsDescription()
    {
        Assert.AreEqual("Test incidence description", _request.Description);
    }

    [TestMethod]
    public void CreateIncidenceRequest_SetsAttractionId()
    {
        Assert.AreEqual(1, _request.AttractionId);
    }

    [TestMethod]
    public void ToEntity_WithoutId_CreatesIncidenceWithDefaultId()
    {
        var entity = _request.ToEntity();

        Assert.AreEqual(0, entity.Id);
    }

    [TestMethod]
    public void ToEntity_WithoutId_SetsDescription()
    {
        var entity = _request.ToEntity();

        Assert.AreEqual("Test incidence description", entity.Description);
    }

    [TestMethod]
    public void ToEntity_WithoutId_SetsAttractionId()
    {
        var entity = _request.ToEntity();

        Assert.AreEqual(1, entity.AttractionId);
    }

    [TestMethod]
    public void ToEntity_WithoutId_SetsActiveToTrue()
    {
        var entity = _request.ToEntity();

        Assert.IsTrue(entity.Active);
    }

    [TestMethod]
    public void ToEntity_WithId_SetsCorrectId()
    {
        var entity = _request.ToEntity(5);

        Assert.AreEqual(5, entity.Id);
    }

    [TestMethod]
    public void ToEntity_WithId_SetsDescription()
    {
        var entity = _request.ToEntity(5);

        Assert.AreEqual("Test incidence description", entity.Description);
    }

    [TestMethod]
    public void ToEntity_WithId_SetsAttractionId()
    {
        var entity = _request.ToEntity(5);

        Assert.AreEqual(1, entity.AttractionId);
    }

    [TestMethod]
    public void ToEntity_WithId_SetsActiveToTrue()
    {
        var entity = _request.ToEntity(5);

        Assert.IsTrue(entity.Active);
    }

    [TestMethod]
    public void ToEntity_WithDifferentDescription_CreatesCorrectEntity()
    {
        var request = new CreateIncidenceRequest
        {
            Description = "Another description",
            AttractionId = 2
        };

        var entity = request.ToEntity();

        Assert.AreEqual("Another description", entity.Description);
    }

    [TestMethod]
    public void ToEntity_WithDifferentAttractionId_CreatesCorrectEntity()
    {
        var request = new CreateIncidenceRequest
        {
            Description = "Test",
            AttractionId = 10
        };

        var entity = request.ToEntity();

        Assert.AreEqual(10, entity.AttractionId);
    }

    [TestMethod]
    public void ToEntity_WithZeroId_SetsIdToZero()
    {
        var entity = _request.ToEntity(0);

        Assert.AreEqual(0, entity.Id);
    }

    [TestMethod]
    public void ToEntity_WithNegativeId_SetsNegativeId()
    {
        var entity = _request.ToEntity(-1);

        Assert.AreEqual(-1, entity.Id);
    }

    [TestMethod]
    public void ToEntity_MultipleCallsSameRequest_CreatesSeparateInstances()
    {
        var entity1 = _request.ToEntity();
        var entity2 = _request.ToEntity();

        Assert.AreNotSame(entity1, entity2);
    }

    [TestMethod]
    public void ToEntity_AlwaysSetsActiveToTrue()
    {
        var request1 = new CreateIncidenceRequest
        {
            Description = "Test 1",
            AttractionId = 1
        };
        var request2 = new CreateIncidenceRequest
        {
            Description = "Test 2",
            AttractionId = 2
        };

        var entity1 = request1.ToEntity();
        var entity2 = request2.ToEntity(10);

        Assert.IsTrue(entity1.Active);
        Assert.IsTrue(entity2.Active);
    }

    [TestMethod]
    public void ToEntityWithMaintenance_SetsMaintenanceDuration()
    {
        var entity = _requestMaintenance.ToEntity();

        Assert.AreEqual(10, entity.Duration);
    }

    [TestMethod]
    public void SetIsMaintenance_GetCorrectly()
    {
        _request.IsMaintenance = true;
        Assert.AreEqual(_request.IsMaintenance, true);
    }

    [TestMethod]
    public void SetStartDate_GetStartDate()
    {
        _request.StartDate = DateTime.Today;
        Assert.AreEqual(_request.StartDate, DateTime.Today);
    }

    [TestMethod]
    public void SetStartTime_GetStartTime()
    {
        var time = DateTime.Now.TimeOfDay;
        _request.StartTime = time;
        Assert.AreEqual(_request.StartTime, time);
    }

    [TestMethod]
    public void SetDuration_GetDuration()
    {
        _request.Duration = 10;
        Assert.AreEqual(_request.Duration, 10);
    }
}
