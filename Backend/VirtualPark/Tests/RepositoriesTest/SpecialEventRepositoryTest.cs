using Abstraction;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Tests.RepositoriesTest;

[TestClass]
public class SpecialEventRepositoryTest
{
    private SqlContext _context = null!;
    private ISpecialEventRepository _repository = null!;

    [TestInitialize]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<SqlContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new SqlContext(options);
        _repository = new SpecialEventRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [TestMethod]
    public void GetByIdWithAttractions_WhenEventExists_ReturnEvent()
    {
        var specialEvent = CreateSpecialEvent();
        _context.SpecialEvents.Add(specialEvent);
        _context.SaveChanges();

        var result = _repository.GetByIdWithAttractions(specialEvent.Id);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetByIdWithAttractions_WhenEventDoesNotExist_ReturnNull()
    {
        var result = _repository.GetByIdWithAttractions(999);

        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetActiveEvents_ReturnOnlyActiveEvents()
    {
        var activeEvent = CreateSpecialEvent();
        var inactiveEvent = CreateSpecialEvent();

        inactiveEvent.IncludedAttractions = [];
        inactiveEvent.Active = false;
        _context.SpecialEvents.AddRange(activeEvent, inactiveEvent);
        _context.SaveChanges();

        var result = _repository.GetActiveEvents();

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetEventsByDate_ReturnEventsForSpecificDate()
    {
        var todayEvent = CreateSpecialEvent();
        todayEvent.Date = DateTime.Today;
        var tomorrowEvent = CreateSpecialEvent();
        tomorrowEvent.IncludedAttractions = [];

        tomorrowEvent.Date = DateTime.Today.AddDays(1);
        _context.SpecialEvents.AddRange(todayEvent, tomorrowEvent);
        _context.SaveChanges();

        var result = _repository.GetEventsByDate(DateTime.Today);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void HasCapacityAvailable_WhenEventHasCapacity_ReturnTrue()
    {
        var specialEvent = CreateSpecialEvent();
        specialEvent.CurrentCapacity = 50;
        specialEvent.MaxCapacity = 100;
        _context.SpecialEvents.Add(specialEvent);
        _context.SaveChanges();

        var result = _repository.HasCapacityAvailable(specialEvent.Id);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void HasCapacityAvailable_WhenEventAtMaxCapacity_ReturnFalse()
    {
        var specialEvent = CreateSpecialEvent();
        specialEvent.CurrentCapacity = 100;
        specialEvent.MaxCapacity = 100;
        _context.SpecialEvents.Add(specialEvent);
        _context.SaveChanges();

        var result = _repository.HasCapacityAvailable(specialEvent.Id);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void HasCapacityAvailable_WhenEventDoesNotExist_ShouldReturnFalse()
    {
        var result = _repository.HasCapacityAvailable(999);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void AreAttractionPartOfAnyEvent_IsTrue()
    {
        var specialEvent = CreateSpecialEvent();
        specialEvent.IncludedAttractions = [new Attraction() { Id = 1 }, new Attraction { Id = 2 }];
        _context.SpecialEvents.Add(specialEvent);
        _context.SaveChanges();

        Assert.IsTrue(_repository.AreAttractionPartOfAnyEvent(1));
    }

    [TestMethod]
    public void AreAttractionPartOfAnyEvent_IsFalse()
    {
        var specialEvent = CreateSpecialEvent();
        specialEvent.IncludedAttractions = [new Attraction { Id = 1 }, new Attraction { Id = 2 }];
        _context.SpecialEvents.Add(specialEvent);
        _context.SaveChanges();

        Assert.IsFalse(_repository.AreAttractionPartOfAnyEvent(3));
    }

    private SpecialEvent CreateSpecialEvent()
    {
        return new SpecialEvent
        {
            Name = "Test Event",
            Date = DateTime.Today.AddDays(10),
            StartTime = new TimeSpan(18, 0, 0),
            EndTime = new TimeSpan(23, 0, 0),
            MaxCapacity = 100,
            CurrentCapacity = 0,
            AdditionalCost = 20,
            IncludedAttractions = [new Attraction { Id = 1 }, new Attraction { Id = 2 }],
            Active = true
        };
    }
}
