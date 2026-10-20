using Abstraction;
using Domain;
using Exceptions;
using Moq;
using NonDomainAbstractions;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class TicketServiceTest
{
    private Mock<ITicketRepository> _repository = null!;
    private Mock<IDateTimeService> _dateTimeService = null!;
    private Mock<IVisitorService> _visitorService = null!;
    private Mock<ISpecialEventService> _specialEventService = null!;
    private ITicketService _ticketService = null!;
    private Visitor _visitor = null!;
    private readonly Visitor _nullVisitor = null!;
    private SpecialEvent _specialEvent = null!;

    [TestInitialize]
    public void Initialize()
    {
        _dateTimeService = new Mock<IDateTimeService>(MockBehavior.Strict);
        _repository = new Mock<ITicketRepository>(MockBehavior.Strict);
        _visitorService = new Mock<IVisitorService>(MockBehavior.Strict);
        _specialEventService = new Mock<ISpecialEventService>(MockBehavior.Strict);

        _dateTimeService.Setup(dt => dt.Now).Returns(DateTime.Now);
        _ticketService = new TicketService(
            _repository.Object,
            _dateTimeService.Object,
            _visitorService.Object,
            _specialEventService.Object);

        _visitor = new Visitor
        {
            Id = 1,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            BirthDate = DateTime.Now.AddYears(-19),
            VisitorParkInformation = new VisitorParkInformation { Id = 1 }
        };

        _specialEvent = new SpecialEvent
        {
            Id = 1,
            Name = "Halloween Night",
            Date = new DateTime(2029, 10, 31),
            StartTime = new TimeSpan(18, 0, 0),
            EndTime = new TimeSpan(23, 0, 0),
            MaxCapacity = 100,
            CurrentCapacity = 50,
            AdditionalCost = 25,
            Active = true
        };
    }

    [TestMethod]
    public void TicketService_IsNotNull()
    {
        Assert.IsNotNull(_ticketService);
    }

    [TestMethod]
    public void CreateTicket_CreateCorrectly()
    {
        _repository.Setup(r => r.Add(It.IsAny<Ticket>()));

        var ticket = new Ticket();

        _ticketService.Create(ticket);

        _repository.VerifyAll();
    }

    [TestMethod]
    public void ConsumeTicket_ConsumeCorrectly()
    {
        var ticket = new Ticket
        {
            Consumed = false,
            Date = DateTime.Now,
            QR = Guid.Empty,
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        _repository.Setup(r => r.ConsumeTicket(ticket));

        _ticketService.ConsumeTicket(ticket);

        _repository.VerifyAll();
    }

    [TestMethod]
    public void ConsumeTicket_ThrowException()
    {
        var ticket = new Ticket
        {
            Consumed = true,
            Date = DateTime.Now,
            QR = Guid.Empty,
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        Assert.ThrowsException<InvalidOperationException>(() => _ticketService.ConsumeTicket(ticket));
    }

    [TestMethod]
    public void GetByTicketQr_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Consumed = false,
            Date = DateTime.Now,
            QR = Guid.Empty,
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        _repository.Setup(r => r.GetTicketByQr(ticket.QR)).Returns(ticket);

        var result = _ticketService.GetTicketByQr(ticket.QR);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetTicketByQr_ThrowsException_WhenTicketNotFound()
    {
        var qr = Guid.NewGuid();
        _repository.Setup(s => s.GetTicketByQr(qr)).Returns((Ticket?)null);

        Assert.ThrowsException<DbKeyDoesNotExistException>(() => _ticketService.GetTicketByQr(qr));
    }

    [TestMethod]
    public void GetById_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Id = 1,
            Consumed = false,
            Date = DateTime.Now,
            QR = Guid.NewGuid(),
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        _repository.Setup(r => r.GetById(1)).Returns(ticket);

        Assert.IsNotNull(_ticketService.GetById(1));
    }

    [TestMethod]
    public void Update_CallRepoUpdate()
    {
        _repository.Setup(r => r.Update(It.IsAny<Ticket>()));

        var ticket = new Ticket();

        _ticketService.Update(ticket);

        _repository.Verify(r => r.Update(ticket), Times.Once);
    }

    [TestMethod]
    public void Delete_CallRepoRemove()
    {
        var ticket = new Ticket();
        _repository.Setup(r => r.GetById(It.IsAny<int>())).Returns(ticket);
        _repository.Setup(r => r.Remove(It.IsAny<Ticket>()));

        _ticketService.Delete(ticket.Id);

        _repository.Verify(r => r.Remove(ticket), Times.Once);
    }

    [TestMethod]
    public void GetAll_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Id = 1,
            Consumed = false,
            Date = DateTime.Now,
            QR = Guid.NewGuid(),
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        var ticket2 = new Ticket
        {
            Id = 2,
            Consumed = false,
            Date = DateTime.Now,
            QR = Guid.NewGuid(),
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 2
        };

        _repository.Setup(r => r.GetAll()).Returns(new List<Ticket> { ticket, ticket2 }.AsQueryable());

        Assert.AreEqual(2, _ticketService.GetAll().Count());
    }

    [TestMethod]
    public void BuyTicket_CreateTicketCorrectly()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _repository.Setup(ts => ts.Add(It.IsAny<Ticket>()));

        _ticketService.BuyTicket(_visitor.Id);

        Mock.VerifyAll();
    }

    [TestMethod]
    public void BuyTicket_ThrowArgumentNullException()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_nullVisitor);

        Assert.ThrowsException<ArgumentNullException>(() => _ticketService.BuyTicket(_visitor.Id));
    }

    [TestMethod]
    public void BuySpecialEventTicket_WithValidData_CreatesTicket()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _specialEventService.Setup(s => s.CanBuyEventTicket(_specialEvent.Id)).Returns(true);
        _repository.Setup(r => r.Add(It.IsAny<Ticket>()));
        _specialEventService.Setup(s => s.IncrementEventCapacity(_specialEvent.Id));

        _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id);

        Mock.VerifyAll();
    }

    [TestMethod]
    public void BuySpecialEventTicket_WithValidData_IncrementsEventCapacity()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _specialEventService.Setup(s => s.CanBuyEventTicket(_specialEvent.Id)).Returns(true);
        _repository.Setup(r => r.Add(It.IsAny<Ticket>()));
        _specialEventService.Setup(s => s.IncrementEventCapacity(_specialEvent.Id));

        _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id);

        _specialEventService.Verify(s => s.IncrementEventCapacity(_specialEvent.Id), Times.Once);
    }

    [TestMethod]
    public void BuySpecialEventTicket_WhenVisitorDoesNotExist_ThrowsException()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_nullVisitor);

        Assert.ThrowsException<DbKeyDoesNotExistException>(() => _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BuySpecialEventTicket_WhenEventNotActive_ThrowsException()
    {
        _specialEvent.Active = false;
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);

        _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id);
    }

    [TestMethod]
    public void BuySpecialEventTicket_WhenEventHasNoCapacity_ThrowsException()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _specialEventService.Setup(s => s.CanBuyEventTicket(_specialEvent.Id)).Returns(false);

        Assert.ThrowsException<InvalidOperationException>(() => _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BuySpecialEventTicket_WhenEventDateInPast_ThrowsException()
    {
        _specialEvent.Date = new DateTime(2025, 10, 5); // Día anterior al Now actual
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _specialEventService.Setup(s => s.CanBuyEventTicket(_specialEvent.Id)).Returns(true);

        _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id);
    }

    [TestMethod]
    public void BuySpecialEventTicket_CreatesTicketWithCorrectType()
    {
        Ticket capturedTicket = null!;

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _specialEventService.Setup(s => s.CanBuyEventTicket(_specialEvent.Id)).Returns(true);
        _repository.Setup(r => r.Add(It.IsAny<Ticket>())).Callback<Ticket>(t => capturedTicket = t);
        _specialEventService.Setup(s => s.IncrementEventCapacity(_specialEvent.Id));

        _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id);

        Assert.AreEqual(nameof(TicketType.SpecialEvent), capturedTicket.Type);
    }

    [TestMethod]
    public void BuySpecialEventTicket_CreatesTicketWithCorrectEventId()
    {
        Ticket capturedTicket = null!;
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _specialEventService.Setup(s => s.CanBuyEventTicket(_specialEvent.Id)).Returns(true);
        _repository.Setup(r => r.Add(It.IsAny<Ticket>())).Callback<Ticket>(t => capturedTicket = t);
        _specialEventService.Setup(s => s.IncrementEventCapacity(_specialEvent.Id));

        _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id);

        Assert.AreEqual(_specialEvent.Id, capturedTicket.SpecialEventId);
    }

    [TestMethod]
    public void BuySpecialEventTicket_CreatesTicketWithEventDate()
    {
        Ticket capturedTicket = null!;

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(_visitor.Id)).Returns(_visitor);
        _specialEventService.Setup(s => s.GetById(_specialEvent.Id)).Returns(_specialEvent);
        _specialEventService.Setup(s => s.CanBuyEventTicket(_specialEvent.Id)).Returns(true);
        _repository.Setup(r => r.Add(It.IsAny<Ticket>())).Callback<Ticket>(t => capturedTicket = t);
        _specialEventService.Setup(s => s.IncrementEventCapacity(_specialEvent.Id));

        _ticketService.BuyTicket(_visitor.Id, _specialEvent.Id);

        Assert.AreEqual(_specialEvent.Date, capturedTicket.Date);
    }
}
