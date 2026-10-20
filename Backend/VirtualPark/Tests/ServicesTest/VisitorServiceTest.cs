using Abstraction;
using Domain;
using Exceptions;
using Moq;
using NonDomainAbstractions;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class VisitorServiceTest
{
    private Mock<IVisitorRepository> _repository = null!;
    private IVisitorService _visitorService = null!;
    private Visitor _visitor = null!;
    private Visitor? _nullVisitor;

    private Mock<IDateTimeService> _dateTimeService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _repository = new Mock<IVisitorRepository>(MockBehavior.Strict);
        _dateTimeService = new Mock<IDateTimeService>(MockBehavior.Strict);
        _dateTimeService.Setup(dt => dt.Now).Returns(new DateTime(2025, 10, 6));

        _visitorService = new VisitorService(
            _repository.Object,
            _dateTimeService.Object);

        _visitor = new Visitor
        {
            Id = 1,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            BirthDate = DateTime.Now,
            VisitorParkInformation = new VisitorParkInformation { Id = 1 }
        };

        _nullVisitor = null;
    }

    [TestMethod]
    public void CreateVisitorService_IsNotNull()
    {
        Assert.IsNotNull(_visitorService);
    }

    [TestMethod]
    public void CreateVisitor_Ok()
    {
        _repository.Setup(r => r.GetByEmail(It.IsAny<string>())).Returns(_nullVisitor);
        _repository.Setup(r => r.Add(It.IsAny<Visitor>()));
        _visitorService.Create(_visitor);

        _repository.VerifyAll();
    }

    [TestMethod]
    public void RemoveVisitor_Ok()
    {
        _repository.Setup(r => r.Remove(It.IsAny<Visitor>()));
        _repository.Setup(r => r.GetById(_visitor.Id)).Returns(_visitor);

        _visitorService.Delete(_visitor.Id);
    }

    [TestMethod]
    public void GetById_ReturnsVisitor()
    {
        _repository.Setup(r => r.GetById(_visitor.Id)).Returns(_visitor);
        var visitor = _visitorService.GetById(_visitor.Id);

        Assert.AreEqual(_visitor.Id, visitor!.Id);
    }

    [TestMethod]
    public void GetById_WhitNonTrackedId_ThrowException()
    {
        _repository.Setup(r => r.GetById(_visitor.Id)).Returns(_nullVisitor);

        Assert.ThrowsException<ArgumentNullException>(() => _visitorService.GetById(_visitor.Id));
    }

    [TestMethod]
    public void GetAll_ReturnsAllVisitors()
    {
        IQueryable<Visitor> visitors = new List<Visitor> { _visitor }.AsQueryable();

        _repository.Setup(r => r.GetAll()).Returns(visitors);
        _repository.Setup(r => r.GetVisitorWithParkInfo(_visitor.Id)).Returns(_visitor);

        var result = _visitorService.GetAll(null);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetAllWithEmail_ReturnsVisitor()
    {
        IQueryable<Visitor> workers = new List<Visitor> { _visitor }.AsQueryable();

        _repository.Setup(r => r.GetByEmail(_visitor.Email)).Returns(_visitor);
        _repository.Setup(r => r.GetVisitorWithParkInfo(_visitor.Id)).Returns(_visitor);
        var result = _visitorService.GetAll(_visitor.Email);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetAllWithNonTrackedEmail_ReturnsEmptyList()
    {
        IQueryable<Visitor> workers = new List<Visitor> { _visitor }.AsQueryable();

        _repository.Setup(r => r.GetByEmail(_visitor.Email)).Returns(_nullVisitor);

        var result = _visitorService.GetAll(_visitor.Email);

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void UpdateVisitor_Ok()
    {
        _repository.Setup(r => r.Update(It.IsAny<Visitor>(), false));

        _visitorService.Update(_visitor);

        _repository.VerifyAll();
    }

    [TestMethod]
    public void UpdateVisitorWithNewPassword_Ok()
    {
        _repository.Setup(r => r.Update(It.IsAny<Visitor>(), true));
        _repository.Setup(r => r.GetById(_visitor.Id)).Returns(_visitor);
        _visitorService.Update(_visitor, "1234ASDe@");
        _repository.VerifyAll();
    }

    [TestMethod]
    public void UpdateVisitorWithNewWrongPassword_ThrowsInvalidOperationException()
    {
        _repository.Setup(r => r.Update(It.IsAny<Visitor>(), false));
        _repository.Setup(r => r.GetById(_visitor.Id)).Returns(_visitor);
        Assert.ThrowsException<InvalidOperationException>(() => _visitorService.Update(_visitor, "Juanson"));
    }

    [TestMethod]
    public void UpdateVisitorParkInfo_Ok()
    {
        _repository.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));

        _visitorService.UpdateVisitorParkInformation(_visitor.VisitorParkInformation);

        _repository.VerifyAll();
    }

    [TestMethod]
    public void UpdateVisitorInvalidId_ThrowArgumentNullException()
    {
        _repository.Setup(r => r.Update(It.IsAny<Visitor>(), false));
        _visitor.Id = 0;
        Assert.ThrowsException<ArgumentNullException>(() => _visitorService.Update(_visitor));
    }

    [TestMethod]
    public void GetVisitorNFC_GetCorrectly()
    {
        _repository.Setup(r => r.GetVisitorWithParkInfo(_visitor.Id)).Returns(_visitor);

        var result = _visitorService.GetVisitorNFC(_visitor.Id);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetVisitorWithParkInformation_GetCorrectly()
    {
        _repository.Setup(r => r.GetVisitorWithParkInfo(_visitor.Id)).Returns(_visitor);

        var result = _visitorService.GetVisitorWithParkInformation(_visitor.Id);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetVisitorWithParkInformation_ThrowException()
    {
        _repository.Setup(r => r.GetVisitorWithParkInfo(_visitor.Id)).Returns(_nullVisitor);

        Assert.ThrowsException<ArgumentNullException>(() => _visitorService.GetVisitorWithParkInformation(_visitor.Id));
    }

    [TestMethod]
    public void GetByEmail_GetCorrectly()
    {
        _repository.Setup(r => r.GetByEmail(_visitor.Email)).Returns(_visitor);

        var result = _visitorService.GetByEmail(_visitor.Email);

        Assert.AreEqual(_visitor.Email, result!.Email);
    }

    [TestMethod]
    public void GetAvailableTickets_GetCorrectly()
    {
        var currentDate = new DateTime(2025, 10, 6);
        var ticket = new Ticket
        {
            Date = currentDate
        };

        var tickets = new List<Ticket> { ticket }.AsQueryable();

        _repository.Setup(r => r.GetAvailableTicketsByNfc(_visitor.VisitorParkInformation.NFC))
            .Returns(tickets);
        _dateTimeService.Setup(dt => dt.Now).Returns(currentDate);

        var result = _visitorService.GetAvailableTicketsByNfc(_visitor.GetIdentifierNFC());

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetVisitsBetweenDates_ReturnsCorrectTotal()
    {
        var startDate = DateTime.Parse("10/10/2010");
        var endDate = DateTime.Parse("11/10/2010");
        var attractionId = 1;
        var expectedVisits = 1;

        _repository
            .Setup(repo => repo.GetVisitsBetween(attractionId, startDate, endDate))
            .Returns(expectedVisits);
        var result = _visitorService.GetVisitsBetween(attractionId, startDate, endDate);

        _repository.Verify(
            repo => repo.GetVisitsBetween(attractionId, startDate, endDate),
            Times.Once);
        Assert.AreEqual(expectedVisits, result);
    }

    [TestMethod]
    public void CreateVisitor_WithRepeatedEmail_ThrowDbCreatedUniqueOrKeyException()
    {
        _repository.Setup(r => r.GetByEmail(_visitor.Email)).Returns(_visitor);
        _repository.Setup(r => r.Add(It.IsAny<Visitor>()));

        Assert.ThrowsException<DbCreatedUniqueOrKeyException>(() => _visitorService.Create(_visitor));
    }

    [TestMethod]
    public void GetAvailableTicketsById_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Date = DateTime.Now
        };

        var tickets = new List<Ticket> { ticket }.AsQueryable();

        _dateTimeService.Setup(dt => dt.Now).Returns(DateTime.Now);

        _repository.Setup(r => r.GetAvailableTicketsById(_visitor.Id))
            .Returns(tickets);

        var result = _visitorService.GetAvailableTicketsById(_visitor.Id);

        Assert.AreEqual(tickets.Count(), result.Count());
    }
}
