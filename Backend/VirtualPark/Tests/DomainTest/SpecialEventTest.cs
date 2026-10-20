using Domain;

namespace Tests.DomainTest;

[TestClass]
public class SpecialEventTest
{
    [TestMethod]
    public void Constructor_InitializesNameAsEmptyString()
    {
        var specialEvent = new SpecialEvent();
        Assert.AreEqual(string.Empty, specialEvent.Name);
    }

    [TestMethod]
    public void Constructor_InitializesIncludedAttractionIdsAsEmptyList()
    {
        var specialEvent = new SpecialEvent();
        Assert.AreEqual(0, specialEvent.IncludedAttractions.Count);
    }

    [TestMethod]
    public void Constructor_InitializesActiveAsTrue()
    {
        var specialEvent = new SpecialEvent();
        Assert.IsTrue(specialEvent.Active);
    }

    [TestMethod]
    public void Constructor_InitializesCurrentCapacityAsZero()
    {
        var specialEvent = new SpecialEvent();
        Assert.AreEqual(0, specialEvent.CurrentCapacity);
    }

    [TestMethod]
    public void Id_SetsAndGetsCorrectly()
    {
        var specialEvent = new SpecialEvent { Id = 5 };
        Assert.AreEqual(5, specialEvent.Id);
    }

    [TestMethod]
    public void Name_SetsAndGetsCorrectly()
    {
        var specialEvent = new SpecialEvent { Name = "Noche de Dinosaurios" };
        Assert.AreEqual("Noche de Dinosaurios", specialEvent.Name);
    }

    [TestMethod]
    public void Date_SetsAndGetsCorrectly()
    {
        var date = new DateTime(2025, 10, 31);
        var specialEvent = new SpecialEvent { Date = date };
        Assert.AreEqual(date, specialEvent.Date);
    }

    [TestMethod]
    public void StartTime_SetsAndGetsCorrectly()
    {
        var time = new TimeSpan(18, 0, 0);
        var specialEvent = new SpecialEvent { StartTime = time };
        Assert.AreEqual(time, specialEvent.StartTime);
    }

    [TestMethod]
    public void EndTime_SetsAndGetsCorrectly()
    {
        var time = new TimeSpan(23, 0, 0);
        var specialEvent = new SpecialEvent { EndTime = time };
        Assert.AreEqual(time, specialEvent.EndTime);
    }

    [TestMethod]
    public void MaxCapacity_SetsAndGetsCorrectly()
    {
        var specialEvent = new SpecialEvent { MaxCapacity = 100 };
        Assert.AreEqual(100, specialEvent.MaxCapacity);
    }

    [TestMethod]
    public void CurrentCapacity_SetsAndGetsCorrectly()
    {
        var specialEvent = new SpecialEvent { CurrentCapacity = 50 };
        Assert.AreEqual(50, specialEvent.CurrentCapacity);
    }

    [TestMethod]
    public void AdditionalCost_SetsAndGetsCorrectly()
    {
        var specialEvent = new SpecialEvent { AdditionalCost = 15.50m };
        Assert.AreEqual(15.50m, specialEvent.AdditionalCost);
    }

    [TestMethod]
    public void IncludedAttractionIds_SetsAndGetsCorrectly()
    {
        var ids = new List<Attraction> { new Attraction(), new Attraction(), new Attraction() };
        var specialEvent = new SpecialEvent { IncludedAttractions = ids };

        Assert.AreEqual(3, specialEvent.IncludedAttractions.Count);
    }

    [TestMethod]
    public void Active_SetsAndGetsCorrectly()
    {
        var specialEvent = new SpecialEvent { Active = false };
        Assert.IsFalse(specialEvent.Active);
    }

    [TestMethod]
    public void IsCapacityAvailable_WhenCurrentLessThanMax_ReturnsTrue()
    {
        var specialEvent = new SpecialEvent { MaxCapacity = 10, CurrentCapacity = 5 };
        Assert.IsTrue(specialEvent.IsCapacityAvailable());
    }

    [TestMethod]
    public void IsCapacityAvailable_WhenCurrentEqualsMax_ReturnsFalse()
    {
        var specialEvent = new SpecialEvent { MaxCapacity = 10, CurrentCapacity = 10 };
        Assert.IsFalse(specialEvent.IsCapacityAvailable());
    }

    [TestMethod]
    public void IncrementCapacity_WhenCapacityAvailable_ReturnsTrue()
    {
        var specialEvent = new SpecialEvent { MaxCapacity = 10, CurrentCapacity = 5 };
        Assert.IsTrue(specialEvent.IncrementCapacity());
    }

    [TestMethod]
    public void IncrementCapacity_WhenCapacityAvailable_IncreasesCurrentCapacity()
    {
        var specialEvent = new SpecialEvent { MaxCapacity = 10, CurrentCapacity = 5 };
        specialEvent.IncrementCapacity();
        Assert.AreEqual(6, specialEvent.CurrentCapacity);
    }

    [TestMethod]
    public void IncrementCapacity_WhenCapacityNotAvailable_ReturnsFalse()
    {
        var specialEvent = new SpecialEvent { MaxCapacity = 10, CurrentCapacity = 10 };
        Assert.IsFalse(specialEvent.IncrementCapacity());
    }

    [TestMethod]
    public void IncrementCapacity_WhenCapacityNotAvailable_DoesNotIncreaseCurrentCapacity()
    {
        var specialEvent = new SpecialEvent { MaxCapacity = 10, CurrentCapacity = 10 };
        specialEvent.IncrementCapacity();
        Assert.AreEqual(10, specialEvent.CurrentCapacity);
    }

    [TestMethod]
    public void DecrementCapacity_WhenCurrentGreaterThanZero_ReturnsTrue()
    {
        var specialEvent = new SpecialEvent { CurrentCapacity = 5 };
        Assert.IsTrue(specialEvent.DecrementCapacity());
    }

    [TestMethod]
    public void DecrementCapacity_WhenCurrentGreaterThanZero_DecreasesCurrentCapacity()
    {
        var specialEvent = new SpecialEvent { CurrentCapacity = 5 };
        specialEvent.DecrementCapacity();
        Assert.AreEqual(4, specialEvent.CurrentCapacity);
    }

    [TestMethod]
    public void DecrementCapacity_WhenCurrentIsZero_ReturnsFalse()
    {
        var specialEvent = new SpecialEvent { CurrentCapacity = 0 };
        Assert.IsFalse(specialEvent.DecrementCapacity());
    }

    [TestMethod]
    public void DecrementCapacity_WhenCurrentIsZero_DoesNotDecreaseCurrentCapacity()
    {
        var specialEvent = new SpecialEvent { CurrentCapacity = 0 };
        specialEvent.DecrementCapacity();
        Assert.AreEqual(0, specialEvent.CurrentCapacity);
    }

    [TestMethod]
    public void IsEventActive_WhenNotActive_ReturnsFalse()
    {
        var specialEvent = new SpecialEvent { Active = false };
        Assert.IsFalse(specialEvent.IsEventActive(DateTime.Now));
    }

    [TestMethod]
    public void IsEventActive_WhenActiveAndWithinTimeRange_ReturnsTrue()
    {
        var specialEvent = new SpecialEvent
        {
            Active = true,
            Date = DateTime.Today,
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(22, 0, 0)
        };
        var testTime = DateTime.Today.AddHours(15);
        Assert.IsTrue(specialEvent.IsEventActive(testTime));
    }

    [TestMethod]
    public void IsEventActive_WhenActiveAndBeforeStartTime_ReturnsFalse()
    {
        var specialEvent = new SpecialEvent
        {
            Active = true,
            Date = DateTime.Today,
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(22, 0, 0)
        };
        var testTime = DateTime.Today.AddHours(9);
        Assert.IsFalse(specialEvent.IsEventActive(testTime));
    }

    [TestMethod]
    public void IsEventActive_WhenActiveAndAfterEndTime_ReturnsFalse()
    {
        var specialEvent = new SpecialEvent
        {
            Active = true,
            Date = DateTime.Today,
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(22, 0, 0)
        };
        var testTime = DateTime.Today.AddHours(23);
        Assert.IsFalse(specialEvent.IsEventActive(testTime));
    }

    [TestMethod]
    public void ContainsAttraction_WhenAttractionIdExists_ReturnsTrue()
    {
        var specialEvent = new SpecialEvent
        {
            IncludedAttractions = [new Attraction(), new Attraction() { Id = 2 }, new Attraction()]
        };
        Assert.IsTrue(specialEvent.ContainsAttraction(2));
    }

    [TestMethod]
    public void ContainsAttraction_WhenAttractionIdDoesNotExist_ReturnsFalse()
    {
        var specialEvent = new SpecialEvent
        {
            IncludedAttractions = [new Attraction()]
        };
        Assert.IsFalse(specialEvent.ContainsAttraction(5));
    }
}
