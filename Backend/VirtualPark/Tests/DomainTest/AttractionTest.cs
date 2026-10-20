using Domain;

namespace Tests.DomainTest;

[TestClass]
public class AttractionTest
{
    private Attraction _attraction = null!;

    [TestInitialize]
    public void Initialize()
    {
        _attraction = new Attraction
        {
            Id = 1,
            Name = "Dragon Coaster",
            TypeAttraction = TypeAttraction.RollerCoaster,
            MinAge = 12,
            MaxCapacity = 40,
            CurrentCapacity = 0,
            Description = "Exciting roller coaster"
        };
    }

    [TestMethod]
    public void CreateAttraction_NotNull()
    {
        Assert.IsNotNull(_attraction);
    }

    [TestMethod]
    public void CreateAttraction_WithEmptyConstructor_NotNull()
    {
        var attraction = new Attraction();
        Assert.IsNotNull(attraction);
    }

    [TestMethod]
    public void SetAttractionId_GetCorrectly()
    {
        _attraction.Id = 5;
        Assert.AreEqual(5, _attraction.Id);
    }

    [TestMethod]
    public void SetAttractionName_GetCorrectly()
    {
        _attraction.Name = "Thunder Mountain";
        Assert.AreEqual("Thunder Mountain", _attraction.Name);
    }

    [TestMethod]
    public void SetAttractionType_GetCorrectly()
    {
        _attraction.TypeAttraction = TypeAttraction.Simulator;
        Assert.AreEqual(TypeAttraction.Simulator, _attraction.TypeAttraction);
    }

    [TestMethod]
    public void SetMinimumAge_GetCorrectly()
    {
        _attraction.MinAge = 8;
        Assert.AreEqual(8, _attraction.MinAge);
    }

    [TestMethod]
    public void SetMaxCapacity_GetCorrectly()
    {
        _attraction.MaxCapacity = 50;
        Assert.AreEqual(50, _attraction.MaxCapacity);
    }

    [TestMethod]
    public void SetCurrentCapacity_GetCorrectly()
    {
        _attraction.CurrentCapacity = 25;
        Assert.AreEqual(25, _attraction.CurrentCapacity);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _attraction.Description = "Nueva descripción";
        Assert.AreEqual("Nueva descripción", _attraction.Description);
    }

    [TestMethod]
    public void Attraction_HasIncidencesList()
    {
        Assert.IsNotNull(_attraction.Incidences);
        Assert.AreEqual(0, _attraction.Incidences.Count);
    }

    [TestMethod]
    public void CreateAttraction_StringsNotNull()
    {
        var attraction = new Attraction();
        Assert.IsNotNull(attraction.Name);
        Assert.IsNotNull(attraction.Description);
        Assert.AreEqual(string.Empty, attraction.Name);
        Assert.AreEqual(string.Empty, attraction.Description);
    }

    [TestMethod]
    public void HasActiveIncidence_WithNoIncidences_ReturnsFalse()
    {
        var result = _attraction.HasActiveIncidence();
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void HasActiveIncidence_WithActiveIncidence_ReturnsTrue()
    {
        var incidence = new Incidence
        {
            Id = 1,
            Description = "Mantenimiento",
            Active = true,
            AttractionId = _attraction.Id
        };
        _attraction.Incidences.Add(incidence);
        var result = _attraction.HasActiveIncidence();
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void HasActiveIncidence_WithOnlyInactiveIncidences_ReturnsFalse()
    {
        var incidence = new Incidence
        {
            Id = 1,
            Description = "Mantenimiento",
            Active = false,
            AttractionId = _attraction.Id
        };
        _attraction.Incidences.Add(incidence);
        var result = _attraction.HasActiveIncidence();
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IsCapacityAvailable_WhenNotFull_ReturnsTrue()
    {
        _attraction.MaxCapacity = 30;
        _attraction.CurrentCapacity = 20;
        var result = _attraction.IsCapacityAvailable();
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IsCapacityAvailable_WhenFull_ReturnsFalse()
    {
        _attraction.MaxCapacity = 30;
        _attraction.CurrentCapacity = 30;
        var result = _attraction.IsCapacityAvailable();
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void GetAvailableCapacity_ReturnsCorrectValue()
    {
        _attraction.MaxCapacity = 40;
        _attraction.CurrentCapacity = 15;
        var result = _attraction.GetAvailableCapacity();
        Assert.AreEqual(25, result);
    }

    [TestMethod]
    public void GetAvailableCapacity_WhenFull_ReturnsZero()
    {
        _attraction.MaxCapacity = 30;
        _attraction.CurrentCapacity = 30;
        var result = _attraction.GetAvailableCapacity();
        Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void IncrementCapacity_WhenSpaceAvailable_IncrementsCurrentCapacity()
    {
        _attraction.MaxCapacity = 30;
        _attraction.CurrentCapacity = 20;
        var result = _attraction.IncrementCapacity();
        Assert.IsTrue(result);
        Assert.AreEqual(21, _attraction.CurrentCapacity);
    }

    [TestMethod]
    public void IncrementCapacity_WhenFull_ReturnsFalseAndDoesNotIncrement()
    {
        _attraction.MaxCapacity = 30;
        _attraction.CurrentCapacity = 30;
        var result = _attraction.IncrementCapacity();
        Assert.IsFalse(result);
        Assert.AreEqual(30, _attraction.CurrentCapacity);
    }

    [TestMethod]
    public void DecrementCapacity_WhenNotEmpty_DecrementsCurrentCapacity()
    {
        _attraction.CurrentCapacity = 20;
        var result = _attraction.DecrementCapacity();
        Assert.IsTrue(result);
        Assert.AreEqual(19, _attraction.CurrentCapacity);
    }

    [TestMethod]
    public void DecrementCapacity_WhenEmpty_ReturnsFalseAndDoesNotDecrement()
    {
        _attraction.CurrentCapacity = 0;
        var result = _attraction.DecrementCapacity();
        Assert.IsFalse(result);
        Assert.AreEqual(0, _attraction.CurrentCapacity);
    }

    [TestMethod]
    public void Attraction_CanHaveMultipleIncidences()
    {
        var incidence1 = new Incidence { Id = 1, Active = true };
        var incidence2 = new Incidence { Id = 2, Active = false };
        var incidence3 = new Incidence { Id = 3, Active = true };
        _attraction.Incidences.Add(incidence1);
        _attraction.Incidences.Add(incidence2);
        _attraction.Incidences.Add(incidence3);
        Assert.AreEqual(3, _attraction.Incidences.Count);
        Assert.IsTrue(_attraction.HasActiveIncidence());
    }
}
