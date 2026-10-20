using Abstraction;
using Domain;
using Exceptions;
using Moq;
using NonDomainAbstractions;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class SpecialEventServiceTest
{
    private Mock<ISpecialEventRepository> _mockRepository = null!;
    private Mock<IAttractionRepository> _mockAttractionService = null!;
    private Mock<IDateTimeService> _mockDateTimeService = null!;
    private ISpecialEventService _service = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ISpecialEventRepository>();
        _mockAttractionService = new Mock<IAttractionRepository>();
        _mockDateTimeService = new Mock<IDateTimeService>();
        _mockDateTimeService.Setup(x => x.Now).Returns(new DateTime(2025, 10, 6));
        _service = new SpecialEventService(
            _mockRepository.Object,
            _mockAttractionService.Object,
            _mockDateTimeService.Object);
    }

    [TestMethod]
    public void GetById_WhenEventExists_ReturnEvent()
    {
        var expectedEvent = new SpecialEvent { Id = 1, Name = "Test Event" };
        _mockRepository.Setup(x => x.GetById(1)).Returns(expectedEvent);

        var result = _service.GetById(1);

        Assert.AreEqual(expectedEvent, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetById_WhenEventDoesNotExist_ThrowException()
    {
        _mockRepository.Setup(x => x.GetById(1)).Returns((SpecialEvent)null!);
        _service.GetById(1);
    }

    [TestMethod]
    public void GetAll_ReturnAllEvents()
    {
        var events = new List<SpecialEvent> { new(), new() };
        _mockRepository.Setup(x => x.GetAll()).Returns(events);

        var result = _service.GetAll();

        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void Create_WithValidEvent_CallRepositoryAdd()
    {
        var specialEvent = CreateValidSpecialEvent();
        _mockAttractionService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Attraction());

        _service.Create(specialEvent);

        _mockRepository.Verify(x => x.Add(specialEvent), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithEmptyName_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Name = " ";
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithNullName_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Name = null!;
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithWhitespaceName_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Name = "   ";
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithZeroMaxCapacity_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.MaxCapacity = 0;
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithNegativeMaxCapacity_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.MaxCapacity = -1;
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithNegativeAdditionalCost_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.AdditionalCost = -10;
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithStartTimeAfterEndTime_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.StartTime = new TimeSpan(22, 0, 0);
        specialEvent.EndTime = new TimeSpan(10, 0, 0);
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithStartTimeEqualToEndTime_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.StartTime = new TimeSpan(10, 0, 0);
        specialEvent.EndTime = new TimeSpan(10, 0, 0);
        _service.Create(specialEvent);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithPastDate_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Date = new DateTime(2025, 10, 5);
        _service.Create(specialEvent);
    }

    [TestMethod]
    public void Create_WithTodayDate_ShouldNotThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Date = new DateTime(2025, 10, 6);
        _mockAttractionService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Attraction());

        _service.Create(specialEvent);

        _mockRepository.Verify(x => x.Add(specialEvent), Times.Once);
    }

    [TestMethod]
    public void Update_WithValidEvent_CallRepositoryUpdate()
    {
        var specialEvent = CreateValidSpecialEvent();
        _mockAttractionService.Setup(x => x.GetById(It.IsAny<int>())).Returns(new Attraction());

        _service.Update(specialEvent);

        _mockRepository.Verify(x => x.Update(specialEvent), Times.Once);
    }

    [TestMethod]
    public void Delete_WhenEventExists_CallRepositoryRemove()
    {
        var specialEvent = new SpecialEvent { Id = 1 };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.Delete(1);

        _mockRepository.Verify(x => x.Remove(specialEvent), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void Delete_WhenEventDoesNotExist_ThrowException()
    {
        _mockRepository.Setup(x => x.GetById(1)).Returns((SpecialEvent)null!);
        _service.Delete(1);
    }

    [TestMethod]
    public void DeactivateEvent_SetActiveToFalse()
    {
        var specialEvent = new SpecialEvent { Id = 1, Active = true };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.DeactivateEvent(1);

        Assert.IsFalse(specialEvent.Active);
    }

    [TestMethod]
    public void DeactivateEvent_CallRepositoryUpdate()
    {
        var specialEvent = new SpecialEvent { Id = 1, Active = true };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.DeactivateEvent(1);

        _mockRepository.Verify(x => x.Update(specialEvent), Times.Once);
    }

    [TestMethod]
    public void IsEventActive_ReturnEventIsActiveResult()
    {
        var specialEvent = new SpecialEvent
        {
            Id = 1,
            Active = true,
            Date = DateTime.Today,
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(22, 0, 0)
        };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);
        var testTime = DateTime.Today.AddHours(15);

        var result = _service.IsEventActive(1, testTime);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void AddAttractionToEvent_WhenAttractionNotInEvent_AddAttraction()
    {
        var specialEvent = new SpecialEvent { Id = 1, IncludedAttractions = [] };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);
        _mockAttractionService.Setup(x => x.GetById(2)).Returns(new Attraction { Id = 2 });

        _service.AddAttractionToEvent(1, 2);

        Assert.IsTrue(specialEvent.IncludedAttractions.Any(a => a.Id == 2));
    }

    [TestMethod]
    public void AddAttractionToEvent_WhenAttractionAlreadyInEvent_NotDuplicate()
    {
        var specialEvent = new SpecialEvent { Id = 1, IncludedAttractions = [new Attraction { Id = 2 }] };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);
        _mockAttractionService.Setup(x => x.GetById(2)).Returns(new Attraction { Id = 2 });

        _service.AddAttractionToEvent(1, 2);

        Assert.AreEqual(1, specialEvent.IncludedAttractions.Count(a => a.Id == 2));
    }

    [TestMethod]
    public void RemoveAttractionFromEvent_RemoveAttraction()
    {
        var specialEvent = new SpecialEvent { Id = 1, IncludedAttractions = [new Attraction { Id = 2 }, new Attraction { Id = 3 }] };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.RemoveAttractionFromEvent(1, 2);

        Assert.IsFalse(specialEvent.IncludedAttractions.Any(a => a.Id == 2));
    }

    [TestMethod]
    public void RemoveAttractionFromEvent_CallRepositoryUpdate()
    {
        var specialEvent = new SpecialEvent { Id = 1, IncludedAttractions = [new Attraction { Id = 2 }] };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.RemoveAttractionFromEvent(1, 2);

        _mockRepository.Verify(x => x.Update(specialEvent), Times.Once);
    }

    [TestMethod]
    public void GetActiveEvents_ReturnActiveEvents()
    {
        var events = new List<SpecialEvent> { new(), new() };
        _mockRepository.Setup(x => x.GetActiveEvents()).Returns(events);

        var result = _service.GetActiveEvents();

        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void CanBuyEventTicket_WhenCapacityAvailable_ReturnTrue()
    {
        _mockRepository.Setup(x => x.HasCapacityAvailable(1)).Returns(true);

        var result = _service.CanBuyEventTicket(1);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CanBuyEventTicket_WhenNoCapacityAvailable_ReturnFalse()
    {
        _mockRepository.Setup(x => x.HasCapacityAvailable(1)).Returns(false);

        var result = _service.CanBuyEventTicket(1);

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IncrementEventCapacity_WhenCapacityAvailable_Increment()
    {
        var specialEvent = new SpecialEvent { Id = 1, MaxCapacity = 10, CurrentCapacity = 5 };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.IncrementEventCapacity(1);

        Assert.AreEqual(6, specialEvent.CurrentCapacity);
    }

    [TestMethod]
    public void IncrementEventCapacity_WhenCapacityAvailable_CallRepositoryUpdate()
    {
        var specialEvent = new SpecialEvent { Id = 1, MaxCapacity = 10, CurrentCapacity = 5 };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.IncrementEventCapacity(1);

        _mockRepository.Verify(x => x.Update(specialEvent), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void IncrementEventCapacity_WhenAtMaxCapacity_ThrowException()
    {
        var specialEvent = new SpecialEvent { Id = 1, MaxCapacity = 10, CurrentCapacity = 10 };
        _mockRepository.Setup(x => x.GetById(1)).Returns(specialEvent);

        _service.IncrementEventCapacity(1);
    }

    [TestMethod]
    public void AreAttractionPartOfAnyEvent_ReturnTrue()
    {
        _mockRepository.Setup(x => x.AreAttractionPartOfAnyEvent(1)).Returns(true);
        Assert.IsTrue(_service.AreAttractionPartOfAnyEvent(1));
    }

    [TestMethod]
    public void DecrementEventCapacity_CallsDecrementAndRepositoryUpdate()
    {
        var sp = CreateValidSpecialEvent();
        _mockRepository.Setup(r => r.GetById(sp.Id)).Returns(sp);
        _service.DecrementEventCapacity(sp.Id);
    }

    private SpecialEvent CreateValidSpecialEvent()
    {
        return new SpecialEvent
        {
            Name = "Test Event",
            Date = new DateTime(2025, 10, 31),
            StartTime = new TimeSpan(18, 0, 0),
            EndTime = new TimeSpan(23, 0, 0),
            MaxCapacity = 100,
            AdditionalCost = 20,
            IncludedAttractions = [new Attraction { Id = 1 }, new Attraction { Id = 2 }],
        };
    }

    [TestMethod]
    public void AddAttractions_AddCorrectly()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Id = 1;

        var attraction1 = new Attraction { Id = 10, Name = "Montaña Rusa" };
        var attraction2 = new Attraction { Id = 20, Name = "Carrusel" };
        var attractionsToAdd = new List<int> { attraction1.Id, attraction2.Id };

        _mockRepository.Setup(x => x.GetById(specialEvent.Id)).Returns(specialEvent);
        _mockAttractionService.Setup(x => x.GetById(10)).Returns(attraction1);
        _mockAttractionService.Setup(x => x.GetById(20)).Returns(attraction2);
        _mockRepository.Setup(s => s.Update(specialEvent));

        _service.AddAttractions(specialEvent.Id, attractionsToAdd);

        Mock.VerifyAll();
    }

    [TestMethod]
    public void AddAttractions_WhenAttractionDoesNotExist_ThrowException()
    {
        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Id = 1;

        _mockRepository.Setup(x => x.GetById(specialEvent.Id)).Returns(specialEvent);
        _mockAttractionService.Setup(x => x.GetById(10)).Returns((Attraction)null!);

        Assert.ThrowsException<DbKeyDoesNotExistException>(
            () => _service.AddAttractions(specialEvent.Id, [10]));
    }

    [TestMethod]
    public void AddAttractions_WithAlreadyIncludedAttraction_DontRepeatAttraction()
    {
        var attraction1 = new Attraction { Id = 10, Name = "Montaña Rusa" };
        var attractionsToAdd = new List<int> { attraction1.Id };

        var specialEvent = CreateValidSpecialEvent();
        specialEvent.Id = 1;
        specialEvent.IncludedAttractions = [attraction1];

        _mockRepository.Setup(x => x.GetById(specialEvent.Id)).Returns(specialEvent);
        _mockAttractionService.Setup(x => x.GetById(10)).Returns(attraction1);
        _mockRepository.Setup(s => s.Update(specialEvent));

        _service.AddAttractions(specialEvent.Id, attractionsToAdd);

        Assert.AreEqual(1, specialEvent.IncludedAttractions.Count);
    }

    [TestMethod]
    public void AddAttractions_WithNameParameter_AddAttractions()
    {
        var attraction1 = new Attraction { Id = 10, Name = "Montaña Rusa" };
        var attractionsToAdd = new List<int> { attraction1.Id };

        var specialEvent = CreateValidSpecialEvent();
        specialEvent.IncludedAttractions = [];
        specialEvent.Name = "Test Event";
        specialEvent.Id = 1;

        _mockRepository.Setup(r => r.GetAll()).Returns([specialEvent]);
        _mockRepository.Setup(x => x.GetById(specialEvent.Id)).Returns(specialEvent);
        _mockRepository.Setup(x => x.GetByIdWithAttractions(specialEvent.Id)).Returns(specialEvent);
        _mockAttractionService.Setup(x => x.GetById(10)).Returns(attraction1);
        _mockRepository.Setup(s => s.Update(specialEvent));

        _service.AddAttractions(specialEvent.Name, attractionsToAdd);

        Assert.AreEqual(1, specialEvent.IncludedAttractions.Count);
    }

    [TestMethod]
    public void AddAttractions_WithNameParameterAndNonTrackedName_ThrowException()
    {
        _mockRepository.Setup(r => r.GetAll()).Returns([]);
        Assert.ThrowsException<DbKeyDoesNotExistException>(() => _service.AddAttractions("Test Event", [1]));
    }
}
