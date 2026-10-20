using Domain;
using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class CreateSpecialEventRequestTest
{
    private CreateSpecialEventRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new CreateSpecialEventRequest
        {
            Name = "Fiesta Nocturna",
            Date = new DateTime(2025, 12, 31),
            StartTime = "20:00:00",
            EndTime = "02:00:00",
            MaxCapacity = 500,
            AdditionalCost = 25.50m,
            AttractionsIds = []
        };
    }

    [TestMethod]
    public void CreateSpecialEventRequest_IsNotNull()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _request.Name = "Concierto de Verano";
        Assert.AreEqual("Concierto de Verano", _request.Name);
    }

    [TestMethod]
    public void SetDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 10, 31);
        _request.Date = newDate;
        Assert.AreEqual(newDate, _request.Date);
    }

    [TestMethod]
    public void SetStartTime_GetCorrectly()
    {
        _request.StartTime = "18:30:00";
        Assert.AreEqual("18:30:00", _request.StartTime);
    }

    [TestMethod]
    public void SetEndTime_GetCorrectly()
    {
        _request.EndTime = "23:45:00";
        Assert.AreEqual("23:45:00", _request.EndTime);
    }

    [TestMethod]
    public void SetMaxCapacity_GetCorrectly()
    {
        _request.MaxCapacity = 1000;
        Assert.AreEqual(1000, _request.MaxCapacity);
    }

    [TestMethod]
    public void SetAdditionalCost_GetCorrectly()
    {
        _request.AdditionalCost = 30.75m;
        Assert.AreEqual(30.75m, _request.AdditionalCost);
    }

    [TestMethod]
    public void SetAttractionIds_GetCorrectly()
    {
        _request.AttractionsIds = [1, 2, 3];
        Assert.AreEqual(_request.AttractionsIds.Count, 3);
    }

    [TestMethod]
    public void ToEntity_WithoutId_CreatesEntityWithDefaultId()
    {
        var entity = _request.ToEntity();
        Assert.IsInstanceOfType(entity, typeof(SpecialEvent));
    }

    [TestMethod]
    public void ToEntity_SetsCurrentCapacityToZero()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(0, entity.CurrentCapacity);
    }

    [TestMethod]
    public void ToEntity_SetsActiveToTrue()
    {
        var entity = _request.ToEntity();
        Assert.IsTrue(entity.Active);
    }

    [TestMethod]
    public void ToEntity_WithId_CreatesEntityWithSpecifiedId()
    {
        var entity = _request.ToEntity(10);
        Assert.AreEqual(10, entity.Id);
    }

    [TestMethod]
    public void ToEntity_MapsNameCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(_request.Name, entity.Name);
    }

    [TestMethod]
    public void ToEntity_MapsDateCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(_request.Date, entity.Date);
    }

    [TestMethod]
    public void ToEntity_MapsStartTimeCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(TimeSpan.Parse(_request.StartTime), entity.StartTime);
    }

    [TestMethod]
    public void ToEntity_MapsEndTimeCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(TimeSpan.Parse(_request.EndTime), entity.EndTime);
    }

    [TestMethod]
    public void ToEntity_MapsMaxCapacityCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(_request.MaxCapacity, entity.MaxCapacity);
    }

    [TestMethod]
    public void ToEntity_MapsAdditionalCostCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(_request.AdditionalCost, entity.AdditionalCost);
    }

    [TestMethod]
    public void ToEntity_ParsesStartTimeFromStringCorrectly()
    {
        _request.StartTime = "14:30:00";
        var entity = _request.ToEntity();
        Assert.AreEqual(new TimeSpan(14, 30, 0), entity.StartTime);
    }

    [TestMethod]
    public void ToEntity_ParsesEndTimeFromStringCorrectly()
    {
        _request.EndTime = "18:45:30";
        var entity = _request.ToEntity();
        Assert.AreEqual(new TimeSpan(18, 45, 30), entity.EndTime);
    }
}
