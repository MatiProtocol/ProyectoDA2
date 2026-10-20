using Abstraction;
using Domain;
using Exceptions;
using Moq;
using NonDomainAbstractions;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class AttractionServiceTest
{
    private Mock<IAttractionRepository> _repository = null!;
    private Mock<ITicketService> _ticketService = null!;
    private Mock<IDateTimeService> _dateTimeService = null!;
    private Mock<IVisitorService> _visitorService = null!;
    private Mock<ISpecialEventService> _specialEventService = null!;

    private IAttractionService _attractionService = null!;
    private Attraction _attraction = null!;
    private readonly Attraction _nullAttraction = null!;
    private SpecialEvent _specialEvent = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repository = new Mock<IAttractionRepository>();
        _ticketService = new Mock<ITicketService>();
        _dateTimeService = new Mock<IDateTimeService>();
        _visitorService = new Mock<IVisitorService>();
        _specialEventService = new Mock<ISpecialEventService>();

        _attractionService = new AttractionService(
            _repository.Object,
            _ticketService.Object,
            _dateTimeService.Object,
            _visitorService.Object,
            _specialEventService.Object);

        _attraction = new Attraction
        {
            Id = 1,
            Name = "Test Attraction",
            MaxCapacity = 100,
            CurrentCapacity = 50,
            MinAge = 12,
            TypeAttraction = TypeAttraction.RollerCoaster
        };

        _specialEvent = new SpecialEvent
        {
            Id = 1,
            Active = true,
            Date = DateTime.Today,
            StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(22, 0, 0),
            IncludedAttractions = [new Attraction { Id = 1 }, new Attraction { Id = 2 }]
        };
    }

    [TestMethod]
    public void CreateAttraction_WithValidData_CallsRepositoryAdd()
    {
        _repository.Setup(r => r.Add(It.IsAny<Attraction>()));
        _repository.Setup(r => r.GetAttractionByName(_attraction.Name)).Returns(_nullAttraction);
        _attractionService.Create(_attraction);
        Mock.VerifyAll();
    }

    [TestMethod]
    public void CreateAttraction_WithEmptyName_ThrowsException()
    {
        var invalidAttraction = new Attraction { Name = " ", MaxCapacity = 10, MinAge = 0 };
        Assert.ThrowsException<ArgumentException>(() => _attractionService.Create(invalidAttraction));
    }

    [TestMethod]
    public void CreateAttraction_WithZeroCapacity_ThrowsException()
    {
        var invalidAttraction = new Attraction { Name = "Test", MaxCapacity = 0, MinAge = 0 };
        Assert.ThrowsException<ArgumentException>(() => _attractionService.Create(invalidAttraction));
    }

    [TestMethod]
    public void CreateAttraction_WithNegativeAge_ThrowsException()
    {
        var invalidAttraction = new Attraction { Name = "Test", MaxCapacity = 10, MinAge = -1 };
        Assert.ThrowsException<ArgumentException>(() => _attractionService.Create(invalidAttraction));
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsAttraction()
    {
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        var result = _attractionService.GetById(_attraction.Id);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsCorrectAttraction()
    {
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        var result = _attractionService.GetById(_attraction.Id);
        Assert.AreEqual(_attraction.Id, result!.Id);
    }

    [TestMethod]
    public void GetById_WithNonExistentId_ReturnsException()
    {
        _repository.Setup(r => r.GetById(999)).Returns(_nullAttraction);
        Assert.ThrowsException<InvalidOperationException>(() => _attractionService.GetById(999));
    }

    [TestMethod]
    public void GetAll_ReturnsCorrectNumberOfAttractions()
    {
        var attractions = new List<Attraction> { _attraction, new Attraction() };
        _repository.Setup(r => r.GetAll()).Returns(attractions);
        var result = _attractionService.GetAll();
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void Update_WithValidData_CallsRepositoryUpdate()
    {
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _attractionService.Update(_attraction);
        Mock.VerifyAll();
    }

    [TestMethod]
    public void Update_WithInvalidData_ThrowsException()
    {
        var invalidAttraction = new Attraction { Name = " ", MaxCapacity = 10, MinAge = 0 };
        Assert.ThrowsException<ArgumentException>(() => _attractionService.Update(invalidAttraction));
    }

    [TestMethod]
    public void Delete_WithExistingId_RemovesAttraction()
    {
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _repository.Setup(r => r.Remove(It.IsAny<Attraction>()));
        _attractionService.Delete(_attraction.Id);
        Mock.VerifyAll();
    }

    [TestMethod]
    public void Delete_WithNonExistentId_OkResult()
    {
        _repository.Setup(r => r.GetById(999)).Returns(_nullAttraction);
        _attractionService.Delete(999);
        _repository.VerifyAll();
    }

    [TestMethod]
    public void Delete_WithNegativeId_InvalidOperationException()
    {
        _repository.Setup(r => r.GetById(-1)).Returns(_nullAttraction);
        Assert.ThrowsException<InvalidOperationException>(() => _attractionService.Delete(-1));
    }

    [TestMethod]
    public void GetByType_ReturnsCorrectNumberOfAttractions()
    {
        var attraction1 = new Attraction { Id = 1, TypeAttraction = TypeAttraction.RollerCoaster };
        var attraction3 = new Attraction { Id = 3, TypeAttraction = TypeAttraction.RollerCoaster };
        var attractions = new List<Attraction> { attraction1, attraction3 }.AsQueryable();
        _repository.Setup(r => r.GetByType(TypeAttraction.RollerCoaster)).Returns(attractions);
        var result = _attractionService.GetByType(TypeAttraction.RollerCoaster);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetByType_ReturnsOnlyAttractionsOfSpecifiedType()
    {
        var attraction1 = new Attraction { Id = 1, TypeAttraction = TypeAttraction.RollerCoaster };
        var attraction3 = new Attraction { Id = 3, TypeAttraction = TypeAttraction.RollerCoaster };
        var attractions = new List<Attraction> { attraction1, attraction3 }.AsQueryable();
        _repository.Setup(r => r.GetByType(TypeAttraction.RollerCoaster)).Returns(attractions);
        var result = _attractionService.GetByType(TypeAttraction.RollerCoaster);
        Assert.IsTrue(result.All(a => a.TypeAttraction == TypeAttraction.RollerCoaster));
    }

    [TestMethod]
    public void GetAvailableAttractions_ReturnsCorrectNumberOfAvailableAttractions()
    {
        var availableAttraction = new Attraction { Id = 2, MaxCapacity = 30, CurrentCapacity = 20 };
        var attractions = new List<Attraction> { availableAttraction }.AsQueryable();
        _repository.Setup(r => r.GetAvailableAttractions()).Returns(attractions);
        var result = _attractionService.GetAvailableAttractions();
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetAvailableAttractions_ReturnsOnlyAvailableAttractions()
    {
        var availableAttraction = new Attraction { Id = 2, MaxCapacity = 30, CurrentCapacity = 20 };
        var attractions = new List<Attraction> { availableAttraction }.AsQueryable();
        _repository.Setup(r => r.GetAvailableAttractions()).Returns(attractions);
        var result = _attractionService.GetAvailableAttractions();
        var available = result.First();
        Assert.IsTrue(available.CurrentCapacity < available.MaxCapacity);
    }

    [TestMethod]
    public void GetAttractionsWithoutActiveIncidences_ReturnsCorrectNumber()
    {
        var attractionWithoutIncidence = new Attraction { Id = 2 };
        var attractions = new List<Attraction> { attractionWithoutIncidence }.AsQueryable();
        _repository.Setup(r => r.GetAttractionsWithoutActiveIncidences()).Returns(attractions);
        var result = _attractionService.GetAttractionsWithoutActiveIncidences();
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void IncrementAttractionCapacity_WhenAvailable_IncreasesCapacity()
    {
        _attraction.CurrentCapacity = 10;
        _attraction.MaxCapacity = 20;
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));

        var result = _attractionService.IncrementAttractionCapacity(_attraction.Id);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void IncrementAttractionCapacity_WhenAvailable_UpdatesCapacityCorrectly()
    {
        _attraction.CurrentCapacity = 10;
        _attraction.MaxCapacity = 20;
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _attractionService.IncrementAttractionCapacity(_attraction.Id);
        Assert.AreEqual(11, _attraction.CurrentCapacity);
    }

    [TestMethod]
    public void IncrementAttractionCapacity_WhenFull_ReturnsFalse()
    {
        _attraction.CurrentCapacity = 20;
        _attraction.MaxCapacity = 20;
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        var result = _attractionService.IncrementAttractionCapacity(_attraction.Id);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void IncrementAttractionCapacity_WhenFull_DoesNotChangeCapacity()
    {
        _attraction.CurrentCapacity = 20;
        _attraction.MaxCapacity = 20;
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _attractionService.IncrementAttractionCapacity(_attraction.Id);
        Assert.AreEqual(20, _attraction.CurrentCapacity);
    }

    [TestMethod]
    public void IncrementAttractionCapacity_AttractionNotFound_ThrowsArgumentException()
    {
        var attractionId = 10;

        _repository.Setup(r => r.GetById(attractionId)).Returns((Attraction?)null);

        Assert.ThrowsException<ArgumentException>(()
            => _attractionService.IncrementAttractionCapacity(attractionId));
    }

    [TestMethod]
    public void DecrementCapacity_DecrementCorrectly()
    {
        _attraction.CurrentCapacity = 1;
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _repository.Setup(r => r.Update(_attraction));
        _attractionService.DecrementCurrentCapacity(_attraction.Id);
        Mock.VerifyAll();
    }

    [TestMethod]
    public void DecrementCapacity_NonTrackedAttraction_ThrowsException()
    {
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_nullAttraction);
        Assert.ThrowsException<ArgumentException>(() => _attractionService.DecrementCurrentCapacity(_attraction.Id));
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithQr_ValidateCorrectly()
    {
        var qr = Guid.NewGuid();
        var ticket = new Ticket { QR = qr, Consumed = false, VisitorParkInformationId = 1 };

        var visitor = new Visitor
        {
            Id = 1,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            BirthDate = DateTime.Parse("2000/01/01")
        };

        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(false);
        _ticketService.Setup(s => s.GetTicketByQr(qr)).Returns(ticket);
        _visitorService.Setup(s => s.GetById(1)).Returns(visitor);
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _ticketService.Setup(s => s.ConsumeTicket(ticket));
        _dateTimeService.Setup(d => d.Now).Returns(DateTime.Now);
        _repository.Setup(r => r.CreateVisitedAttraction(It.IsAny<VisitedAttraction>()));
        _visitorService.Setup(s => s.GetVisitorWithParkInformation(visitor.Id)).Returns(visitor);

        _attractionService.ValidateAttractionAccess(_attraction.Id, qr, null);

        Mock.VerifyAll();
    }

    [TestMethod]
    public void ValidateAttractionAccess_TicketAlreadyConsumed_ThrowException()
    {
        var qr = Guid.Empty;

        var ticket = new Ticket { QR = qr, Consumed = true };

        var visitor = new Visitor
        {
            Id = 0,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            BirthDate = DateTime.Parse("2013/08/12")
        };

        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(false);
        _ticketService.Setup(s => s.GetTicketByQr(ticket.QR)).Returns(ticket);
        _visitorService.Setup(s => s.GetById(visitor.Id)).Returns(visitor);
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _dateTimeService.Setup(d => d.Now).Returns(DateTime.Now);
        _ticketService.Setup(s => s.ConsumeTicket(ticket)).Throws(new InvalidOperationException());

        Assert.ThrowsException<InvalidOperationException>(() =>
            _attractionService.ValidateAttractionAccess(_attraction.Id, ticket.QR, null));
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithNfc_ValidateCorrectly()
    {
        var nfc = Guid.NewGuid();
        var ticket = new Ticket { QR = Guid.NewGuid(), Consumed = false, VisitorParkInformationId = 1 };

        var visitor = new Visitor
        {
            Id = 1,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            BirthDate = DateTime.Parse("2000/01/01")
        };

        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(false);
        _specialEventService.Setup(s => s.GetActiveEvents()).Returns([]);
        _visitorService.Setup(s => s.GetAvailableTicketsByNfc(nfc)).Returns(new List<Ticket> { ticket }.AsQueryable());
        _visitorService.Setup(s => s.GetById(ticket.VisitorParkInformationId)).Returns(visitor);
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _dateTimeService.Setup(d => d.Now).Returns(DateTime.Now);
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _ticketService.Setup(s => s.ConsumeTicket(ticket));
        _repository.Setup(r => r.CreateVisitedAttraction(It.IsAny<VisitedAttraction>()));
        _visitorService.Setup(s => s.GetVisitorWithParkInformation(visitor.Id)).Returns(visitor);

        _attractionService.ValidateAttractionAccess(_attraction.Id, null, nfc);

        Mock.VerifyAll();
    }

    [TestMethod]
    public void ValidateAttractionAccess_VisitorBirthdayNotYetThisYear_ThrowsMinimumAgeException()
    {
        var attraction = new Attraction
        {
            Id = 10,
            Name = "Montaña Rusa",
            MinAge = 18,
            MaxCapacity = 20,
            CurrentCapacity = 0
        };

        var date = new DateTime(2025, 10, 7);

        var visitor = new Visitor
        {
            Id = 1,
            BirthDate = new DateTime(2007, 12, 15)
        };

        var ticket = new Ticket
        {
            QR = Guid.NewGuid(),
            VisitorParkInformationId = visitor.Id,
            SpecialEventId = null,
            Consumed = false
        };

        var nfc = Guid.NewGuid();

        List<Ticket> tickets = [ticket];

        _repository.Setup(r => r.ExistIncidentActive(attraction.Id)).Returns(false);
        _specialEventService.Setup(s => s.GetActiveEvents()).Returns([]);
        _visitorService.Setup(s => s.GetAvailableTicketsByNfc(nfc)).Returns(tickets.AsQueryable());
        _visitorService.Setup(s => s.GetById(visitor.Id)).Returns(visitor);
        _repository.Setup(r => r.GetById(attraction.Id)).Returns(attraction);
        _dateTimeService.Setup(d => d.Now).Returns(date);

        Assert.ThrowsException<MinimumAgeException>(() =>
        _attractionService.ValidateAttractionAccess(attraction.Id, null, nfc));
    }

    [TestMethod]
    public void ValidateAttractionAccess_NoTicketsByNfc_ThrowsException()
    {
        var attraction = new Attraction
        {
            Id = 10,
            Name = "Montaña Rusa",
            MinAge = 18,
            MaxCapacity = 20,
            CurrentCapacity = 0
        };
        var nfc = Guid.NewGuid();

        List<Ticket> helper = [];

        _repository.Setup(r => r.ExistIncidentActive(attraction.Id)).Returns(false);
        _specialEventService.Setup(s => s.GetActiveEvents()).Returns([]);
        _visitorService.Setup(s => s.GetAvailableTicketsByNfc(nfc)).Returns(helper.AsQueryable());

        Assert.ThrowsException<InvalidOperationException>(() =>
        _attractionService.ValidateAttractionAccess(attraction.Id, null, nfc));
    }

    [TestMethod]
    public void ValidateAttractionAccess_NfcWithSpecialEventTicket_UsesSpecialEventTicket()
    {
        var attraction = new Attraction
        {
            Id = 10,
            Name = "Montaña Rusa",
            MinAge = 18,
            MaxCapacity = 20,
            CurrentCapacity = 0
        };

        var nfc = Guid.NewGuid();
        var visitor = new Visitor
        {
            Id = 1,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            BirthDate = DateTime.Parse("2000/01/01")
        };

        var ticketSpecial = new Ticket
        {
            VisitorParkInformationId = visitor.Id,
            SpecialEventId = 5,
            Consumed = false
        };

        var ticketNormal = new Ticket
        {
            VisitorParkInformationId = visitor.Id,
            SpecialEventId = null,
            Consumed = false
        };

        var specialEvent = new SpecialEvent
        {
            Id = 5,
            IncludedAttractions = [attraction]
        };

        var tickets = new List<Ticket> { ticketNormal, ticketSpecial };

        var specialEvents = new List<SpecialEvent> { specialEvent };

        _repository.Setup(r => r.ExistIncidentActive(attraction.Id)).Returns(false);
        _specialEventService.Setup(s => s.GetActiveEvents()).Returns(specialEvents);
        _visitorService.Setup(s => s.GetAvailableTicketsByNfc(nfc)).Returns(tickets.AsQueryable());
        _visitorService.Setup(s => s.GetById(visitor.Id)).Returns(visitor);
        _repository.Setup(r => r.GetById(attraction.Id)).Returns(attraction);
        _dateTimeService.Setup(d => d.Now).Returns(DateTime.Now);
        _ticketService.Setup(s => s.ConsumeTicket(ticketSpecial));
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _repository.Setup(r => r.CreateVisitedAttraction(It.IsAny<VisitedAttraction>()));
        _visitorService.Setup(s => s.GetVisitorWithParkInformation(visitor.Id)).Returns(visitor);

        _attractionService.ValidateAttractionAccess(attraction.Id, null, nfc);

        Mock.VerifyAll();
    }

    [TestMethod]
    public void ValidateAttractionAccess_NfcNoSpecialEventTicket_UsesNormalTicket()
    {
        var attraction = new Attraction
        {
            Id = 10,
            Name = "Montaña Rusa",
            MinAge = 10,
            MaxCapacity = 20,
            CurrentCapacity = 0
        };

        var nfc = Guid.NewGuid();
        var visitor = new Visitor
        {
            Id = 1,
            BirthDate = DateTime.Parse("2000-01-01")
        };

        var tickets = new List<Ticket>
        {
            new Ticket { VisitorParkInformationId = visitor.Id, SpecialEventId = 9, Consumed = false },
            new Ticket { VisitorParkInformationId = visitor.Id, SpecialEventId = null, Consumed = false }
        };

        var specialEvents = new List<SpecialEvent>
        {
            new SpecialEvent { Id = 5, IncludedAttractions = [attraction] }
        };

        _repository.Setup(r => r.ExistIncidentActive(attraction.Id)).Returns(false);
        _specialEventService.Setup(s => s.GetActiveEvents()).Returns(specialEvents);
        _visitorService.Setup(s => s.GetAvailableTicketsByNfc(nfc)).Returns(tickets.AsQueryable());
        _visitorService.Setup(s => s.GetById(visitor.Id)).Returns(visitor);
        _repository.Setup(r => r.GetById(attraction.Id)).Returns(attraction);
        _dateTimeService.Setup(d => d.Now).Returns(DateTime.Now);
        _ticketService.Setup(s => s.ConsumeTicket(It.Is<Ticket>(t => t.SpecialEventId == null)));
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _repository.Setup(r => r.CreateVisitedAttraction(It.IsAny<VisitedAttraction>()));
        _visitorService.Setup(s => s.GetVisitorWithParkInformation(visitor.Id)).Returns(visitor);

        _attractionService.ValidateAttractionAccess(attraction.Id, null, nfc);

        Mock.VerifyAll();
    }

    [TestMethod]
    public void ValidateAttractionAccess_TicketByQrNotFound_ThrowsException()
    {
        var attraction = new Attraction
        {
            Id = 10,
            Name = "Montaña Rusa",
            MinAge = 10,
            MaxCapacity = 20,
            CurrentCapacity = 0
        };

        var qr = Guid.NewGuid();

        _repository.Setup(r => r.ExistIncidentActive(attraction.Id)).Returns(false);
        _specialEventService.Setup(s => s.GetActiveEvents()).Returns([]);
        _ticketService.Setup(s => s.GetTicketByQr(qr)).Returns((Ticket?)null);
        _repository.Setup(r => r.GetById(attraction.Id)).Returns(attraction);

        Assert.ThrowsException<InvalidOperationException>(() =>
        _attractionService.ValidateAttractionAccess(attraction.Id, qr, null));
    }

    [TestMethod]
    public void ValidateAttractionAccess_VisitorNonMinimumAge_ValidateCorrectly()
    {
        var qr = Guid.NewGuid();
        var ticket = new Ticket { QR = qr, Consumed = false, VisitorParkInformationId = 1 };

        var visitor = new Visitor { Id = 1, BirthDate = DateTime.Now };

        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(false);
        _ticketService.Setup(s => s.GetTicketByQr(qr)).Returns(ticket);
        _visitorService.Setup(s => s.GetById(1)).Returns(visitor);
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _dateTimeService.Setup(d => d.Now).Returns(DateTime.Now);

        Assert.ThrowsException<MinimumAgeException>(() =>
            _attractionService.ValidateAttractionAccess(_attraction.Id, qr, null));
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithoutQrAndNfc_ThrowsArgumentNullException()
    {
        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(false);
        _specialEventService.Setup(s => s.GetActiveEvents()).Returns([]);
        Assert.ThrowsException<InvalidOperationException>(() =>
            _attractionService.ValidateAttractionAccess(_attraction.Id, null, null));
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithIncidences_ThrowsInvalidOperator()
    {
        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(true);
        Assert.ThrowsException<InvalidOperationException>(() =>
            _attractionService.ValidateAttractionAccess(_attraction.Id, null, null));
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithInactiveEvent_ThrowsException()
    {
        var ticket = new Ticket { QR = Guid.NewGuid(), SpecialEventId = _specialEvent.Id };

        var currentTime = DateTime.Today.AddHours(5);

        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(false);
        _ticketService.Setup(s => s.GetTicketByQr(ticket.QR)).Returns(ticket);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _dateTimeService.Setup(d => d.Now).Returns(currentTime);

        Assert.ThrowsException<InvalidOperationException>(() =>
        _attractionService.ValidateAttractionAccess(_attraction.Id, ticket.QR, null));
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithGeneralTicket_DoesNotValidateEventAccess()
    {
        var ticket = new Ticket { QR = Guid.NewGuid(), SpecialEventId = null, VisitorParkInformationId = 1 };

        var visitor = new Visitor { Id = 1, BirthDate = DateTime.Parse("2000/01/01") };

        _repository.Setup(r => r.ExistIncidentActive(_attraction.Id)).Returns(false);
        _ticketService.Setup(s => s.GetTicketByQr(ticket.QR)).Returns(ticket);
        _visitorService.Setup(s => s.GetById(1)).Returns(visitor);
        _repository.Setup(r => r.GetById(_attraction.Id)).Returns(_attraction);
        _dateTimeService.Setup(d => d.Now).Returns(DateTime.Now);
        _repository.Setup(r => r.Update(It.IsAny<Attraction>()));
        _ticketService.Setup(s => s.ConsumeTicket(ticket));
        _repository.Setup(r => r.CreateVisitedAttraction(It.IsAny<VisitedAttraction>()));
        _visitorService.Setup(s => s.GetVisitorWithParkInformation(visitor.Id)).Returns(visitor);

        _attractionService.ValidateAttractionAccess(_attraction.Id, ticket.QR, null);

        _specialEventService.Verify(s => s.GetById(It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public void ValidateAttractionAccess_WithAttractionNotInEvent_ThrowsException()
    {
        var ticket = new Ticket { QR = Guid.NewGuid(), SpecialEventId = _specialEvent.Id };

        var attractionNotInEvent = new Attraction { Id = 99 };
        var currentTime = DateTime.Today.AddHours(15);

        _repository.Setup(r => r.ExistIncidentActive(attractionNotInEvent.Id)).Returns(false);
        _ticketService.Setup(s => s.GetTicketByQr(ticket.QR)).Returns(ticket);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _dateTimeService.Setup(d => d.Now).Returns(currentTime);

        Assert.ThrowsException<InvalidOperationException>(() =>
        _attractionService.ValidateAttractionAccess(attractionNotInEvent.Id, ticket.QR, null));
    }

    [TestMethod]
    public void GetVisitsBetweenDates_ReturnsCorrectTotal()
    {
        var startDate = DateTime.Parse("10/10/2010");
        var endDate = DateTime.Parse("11/10/2010");
        var attractionId = 1;
        var expectedVisits = 1;

        _visitorService
            .Setup(service => service.GetVisitsBetween(attractionId, startDate, endDate))
            .Returns(expectedVisits);

        var result = _attractionService.GetVisitsBetween(attractionId, startDate, endDate);

        Assert.AreEqual(expectedVisits, result);
    }

    [TestMethod]
    public void CreateAttractionService_IsNotNull()
    {
        Assert.IsNotNull(_attractionService);
    }

    [TestMethod]
    public void AttractionService_ImplementsIService()
    {
        Assert.IsInstanceOfType(_attractionService, typeof(IRUService<Attraction>));
    }

    [TestMethod]
    public void Create_WithTrackedName_ThrowException()
    {
        _repository.Setup(r => r.GetAttractionByName(_attraction.Name)).Returns(_attraction);

        Assert.ThrowsException<DbCreatedUniqueOrKeyException>(() => _attractionService.Create(_attraction));
    }
}
