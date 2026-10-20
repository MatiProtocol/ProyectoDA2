using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetSpecialEventResponseTest
{
    private SpecialEvent _specialEvent = null!;
    private GetSpecialEventResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _specialEvent = new SpecialEvent
        {
            Id = 1,
            Name = "Fiesta Nocturna",
            Date = new DateTime(2025, 12, 31),
            StartTime = new TimeSpan(20, 0, 0),
            EndTime = new TimeSpan(2, 0, 0),
            MaxCapacity = 500,
            CurrentCapacity = 250,
            AdditionalCost = 25.50m,
            IncludedAttractions = [new Attraction { Id = 1 }, new Attraction { Id = 2 }, new Attraction { Id = 2 }],
            Active = true
        };
        _response = new GetSpecialEventResponse(_specialEvent);
    }

    [TestMethod]
    public void GetSpecialEventResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsIdCorrectly()
    {
        Assert.AreEqual(_specialEvent.Id, _response.Id);
    }

    [TestMethod]
    public void Constructor_MapsNameCorrectly()
    {
        Assert.AreEqual(_specialEvent.Name, _response.Name);
    }

    [TestMethod]
    public void Constructor_MapsDateCorrectly()
    {
        Assert.AreEqual("2025-12-31", _response.Date);
    }

    [TestMethod]
    public void Constructor_MapsStartTimeCorrectly()
    {
        Assert.AreEqual("20:00", _response.StartTime);
    }

    [TestMethod]
    public void Constructor_MapsEndTimeCorrectly()
    {
        Assert.AreEqual("02:00", _response.EndTime);
    }

    [TestMethod]
    public void Constructor_MapsMaxCapacityCorrectly()
    {
        Assert.AreEqual(_specialEvent.MaxCapacity, _response.MaxCapacity);
    }

    [TestMethod]
    public void Constructor_MapsCurrentCapacityCorrectly()
    {
        Assert.AreEqual(_specialEvent.CurrentCapacity, _response.CurrentCapacity);
    }

    [TestMethod]
    public void Constructor_MapsAdditionalCostCorrectly()
    {
        Assert.AreEqual(_specialEvent.AdditionalCost, _response.AdditionalCost);
    }

    [TestMethod]
    public void Constructor_MapsIncludedAttractionIdsCorrectly()
    {
        var result = _specialEvent.IncludedAttractions.All(a => _response.IncludedAttractionIds.Contains(a.Id));
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void Constructor_MapsActiveCorrectly()
    {
        Assert.AreEqual(_specialEvent.Active, _response.Active);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _response.Id = 10;
        Assert.AreEqual(10, _response.Id);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _response.Name = "Concierto de Verano";
        Assert.AreEqual("Concierto de Verano", _response.Name);
    }

    [TestMethod]
    public void SetDate_GetCorrectly()
    {
        _response.Date = "2025-10-31";
        Assert.AreEqual("2025-10-31", _response.Date);
    }

    [TestMethod]
    public void SetStartTime_GetCorrectly()
    {
        _response.StartTime = "18:30";
        Assert.AreEqual("18:30", _response.StartTime);
    }

    [TestMethod]
    public void SetEndTime_GetCorrectly()
    {
        _response.EndTime = "23:45";
        Assert.AreEqual("23:45", _response.EndTime);
    }

    [TestMethod]
    public void SetMaxCapacity_GetCorrectly()
    {
        _response.MaxCapacity = 1000;
        Assert.AreEqual(1000, _response.MaxCapacity);
    }

    [TestMethod]
    public void SetCurrentCapacity_GetCorrectly()
    {
        _response.CurrentCapacity = 500;
        Assert.AreEqual(500, _response.CurrentCapacity);
    }

    [TestMethod]
    public void SetAdditionalCost_GetCorrectly()
    {
        _response.AdditionalCost = 30.75m;
        Assert.AreEqual(30.75m, _response.AdditionalCost);
    }

    [TestMethod]
    public void SetIncludedAttractionIds_GetCorrectly()
    {
        var newAttractionIds = new List<int> { 4, 5, 6 };
        _response.IncludedAttractionIds = newAttractionIds;
        CollectionAssert.AreEqual(newAttractionIds, _response.IncludedAttractionIds);
    }

    [TestMethod]
    public void SetActive_GetCorrectly()
    {
        _response.Active = false;
        Assert.IsFalse(_response.Active);
    }
}
