using Abstraction;
using Domain;
using Moq;
using NonDomainAbstractions;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class PunctuationServiceTest
{
    private Mock<IVisitorService> _visitorService = null!;
    private Mock<IDateTimeService> _dateTimeService = null!;
    private Mock<IPointsHistoryService> _pointsHistoryService = null!;
    private IPunctuationService _service = null!;
    private Visitor _visitor = null!;
    private PunctuationContext _context = null!;
    private Mock<IServiceProvider> _serviceProvider = null!;

    [TestInitialize]
    public void Initialize()
    {
        _visitorService = new Mock<IVisitorService>(MockBehavior.Strict);
        _dateTimeService = new Mock<IDateTimeService>(MockBehavior.Strict);
        _pointsHistoryService = new Mock<IPointsHistoryService>(MockBehavior.Strict);
        _serviceProvider = new Mock<IServiceProvider>(MockBehavior.Strict);

        _service = new PunctuationService(
            _visitorService.Object,
            _dateTimeService.Object,
            _pointsHistoryService.Object,
            _serviceProvider.Object);

        var ticket = new Ticket
        {
            Id = 1,
            Date = DateTime.Parse("2024/01/01"),
            Type = nameof(TicketType.General)
        };
        var visitedAttraction = new VisitedAttraction
        {
            VisitorParkInformationId = 1,
            AttractionType = TypeAttraction.RollerCoaster,
            VisitDate = DateTime.Now,
            AttractionId = 1,
            IsSpecialEvent = false
        };

        var visitorParkInformation = new VisitorParkInformation
        {
            VisitedAttractions = [visitedAttraction],
            Id = 1,
            Points = 0,
            Tickets = [ticket]
        };

        _visitor = new Visitor
        {
            Id = 1,
            Name = "John",
            Email = "john@test.com",
            VisitorParkInformation = visitorParkInformation
        };

        _context = new PunctuationContext
        {
            VisitorParkInformationId = 1,
            ActualAttraction = visitedAttraction,
            PreviousAccesses = [visitedAttraction],
        };

        _dateTimeService.Setup(ds => ds.Now).Returns(DateTime.Parse("2024/01/01 10:00"));

        _pointsHistoryService.Setup(phs => phs.RecordTransaction(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>())).Verifiable();
    }

    [TestMethod]
    public void Constructor_CreatesInstance()
    {
        Assert.IsNotNull(_service);
    }

    [TestMethod]
    public void Constructor_SetsDefaultStrategy()
    {
        Assert.AreEqual("AttractionBased", _service.GetActiveStrategy());
    }

    [TestMethod]
    public void GetActiveStrategy_ReturnsAttractionBased_WhenDefault()
    {
        Assert.AreEqual("AttractionBased", _service.GetActiveStrategy());
    }

    [TestMethod]
    public void GetActiveStrategy_ReturnsCombo_AfterSettingCombo()
    {
        _service.SetActiveStrategy("Combo");
        Assert.AreEqual("Combo", _service.GetActiveStrategy());
    }

    [TestMethod]
    public void GetActiveStrategy_ReturnsEvent_AfterSettingEvent()
    {
        _service.SetActiveStrategy("Event");
        Assert.AreEqual("Event", _service.GetActiveStrategy());
    }

    [TestMethod]
    public void SetActiveStrategy_ChangesToCombo()
    {
        _service.SetActiveStrategy("Combo");
        Assert.AreEqual("Combo", _service.GetActiveStrategy());
    }

    [TestMethod]
    public void SetActiveStrategy_ChangesToEvent()
    {
        _service.SetActiveStrategy("Event");
        Assert.AreEqual("Event", _service.GetActiveStrategy());
    }

    [TestMethod]
    public void SetActiveStrategy_ChangesBackToAttractionBased()
    {
        _service.SetActiveStrategy("Event");
        _service.SetActiveStrategy("AttractionBased");
        Assert.AreEqual("AttractionBased", _service.GetActiveStrategy());
    }

    [TestMethod]
    public void SetActiveStrategy_ThrowsException_WhenStrategyNotFound()
    {
        Assert.ThrowsException<ArgumentException>(() => _service.SetActiveStrategy("InvalidStrategy"));
    }

    [TestMethod]
    public void SetActiveStrategy_ThrowsException_WhenNull()
    {
        Assert.ThrowsException<ArgumentException>(() => _service.SetActiveStrategy(null!));
    }

    [TestMethod]
    public void SetActiveStrategy_ThrowsException_WhenEmpty()
    {
        Assert.ThrowsException<ArgumentException>(() => _service.SetActiveStrategy(string.Empty));
    }

    [TestMethod]

    public void SetActiveStrategy_ThrowsException_WhenWhitespace()
    {
        Assert.ThrowsException<ArgumentException>(() => _service.SetActiveStrategy("   "));
    }

    [TestMethod]
    public void GetAvailableStrategies_ReturnsThreeStrategies()
    {
        Assert.AreEqual(3, _service.GetAvailableStrategies().Count);
    }

    [TestMethod]
    public void GetAvailableStrategies_ContainsAttractionBased()
    {
        Assert.IsTrue(_service.GetAvailableStrategies().Contains("AttractionBased"));
    }

    [TestMethod]
    public void GetAvailableStrategies_ContainsCombo()
    {
        Assert.IsTrue(_service.GetAvailableStrategies().Contains("Combo"));
    }

    [TestMethod]
    public void GetAvailableStrategies_ContainsEvent()
    {
        Assert.IsTrue(_service.GetAvailableStrategies().Contains("Event"));
    }

    [TestMethod]
    public void CalculatePoints_WithRollerCoaster_Returns100()
    {
        _context.ActualAttractionType = nameof(TypeAttraction.RollerCoaster);
        Assert.AreEqual(100, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_WithSimulator_Returns80()
    {
        _context.ActualAttractionType = nameof(TypeAttraction.Simulator);
        Assert.AreEqual(80, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_WithShow_Returns60()
    {
        _context.ActualAttractionType = nameof(TypeAttraction.Show);
        Assert.AreEqual(60, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_WithInteractiveZone_Returns40()
    {
        _context.ActualAttractionType = nameof(TypeAttraction.InteractiveZone);
        Assert.AreEqual(40, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_ThrowsException_WhenContextIsNull()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _service.CalculatePoints(null!));
    }

    [TestMethod]
    public void CalculatePoints_ComboStrategy_ReturnsBasePoints()
    {
        _service.SetActiveStrategy("Combo");
        _context.PreviousAccesses = [];
        Assert.AreEqual(50, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_ComboStrategy_ReturnsBonusPoints_WithRecentDifferentAttraction()
    {
        _service.SetActiveStrategy("Combo");
        _context.ActualAttractionType = nameof(TypeAttraction.RollerCoaster);
        _context.PreviousAccesses =
        [
            new VisitedAttraction
            {
                AttractionType = TypeAttraction.Simulator,
                VisitDate = DateTime.Now.AddMinutes(-3)
            }

        ];
        Assert.AreEqual(100, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_ComboStrategy_NoBonus_WithSameAttraction()
    {
        _service.SetActiveStrategy("Combo");
        _context.ActualAttractionType = nameof(TypeAttraction.RollerCoaster);
        _context.PreviousAccesses =
        [
            new VisitedAttraction
            {
                AttractionType = TypeAttraction.RollerCoaster,
                VisitDate = DateTime.Now.AddMinutes(-3)
            }

        ];
        Assert.AreEqual(50, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_EventStrategy_ReturnsBasePoints_NoEvent()
    {
        _service.SetActiveStrategy("Event");
        _context.ActualAttractionIsSpecial = false;
        Assert.AreEqual(50, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void CalculatePoints_EventStrategy_ReturnsDoublePoints_WithEvent()
    {
        _service.SetActiveStrategy("Event");
        _context.ActualAttractionIsSpecial = true;
        Assert.AreEqual(100, _service.CalculatePoints(_context));
    }

    [TestMethod]
    public void GetVisitorTotalPoints_ReturnsZero_WhenNoPoints()
    {
        _visitor.VisitorParkInformation.Points = 0;

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        Assert.AreEqual(0, _service.GetVisitorTotalPoints(1, null));
    }

    [TestMethod]
    public void GetVisitorTotalPoints_ReturnsCorrectPoints()
    {
        _visitor.VisitorParkInformation.Points = 250;

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        Assert.AreEqual(250, _service.GetVisitorTotalPoints(1, null));
    }

    [TestMethod]
    public void GetVisitorTotalPointsWithNfc_ReturnsCorrectPoints()
    {
        _visitor.VisitorParkInformation.Points = 250;
        var nfc = Guid.NewGuid();
        _visitor.VisitorParkInformation.NFC = nfc;
        var ticket = new Ticket();
        ticket.VisitorParkInformationId = _visitor.VisitorParkInformation.Id;

        _visitorService.Setup(vs => vs.GetAll(null)).Returns(new List<Visitor> { _visitor }.AsQueryable());
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);

        Assert.AreEqual(250, _service.GetVisitorTotalPoints(null, _visitor.VisitorParkInformation.NFC.ToString()));
    }

    [TestMethod]
    public void GetVisitorTotalPoints_ThrowsException_WhenVisitorNotFound()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(999)).Returns(((Visitor?)null)!);
        Assert.ThrowsException<ArgumentException>(() => _service.GetVisitorTotalPoints(999, null));
    }

    [TestMethod]
    public void AddPointsToVisitor_AddsPointsCorrectly()
    {
        _visitor.VisitorParkInformation.Points = 100;
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));
        _service.AddPointsToVisitor(1, 50);
        Assert.AreEqual(150, _visitor.VisitorParkInformation.Points);
    }

    [TestMethod]
    public void AddPointsToVisitor_ThrowsException_WhenVisitorNotFound()
    {
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(999)).Returns(((Visitor?)null)!);
        Assert.ThrowsException<ArgumentException>(() => _service.AddPointsToVisitor(999, 50));
    }

    [TestMethod]
    public void ProcessAttractionAccess_ReturnsCorrectPoints_ForRollerCoaster()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1
        };
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));
        _service.SetActiveStrategy("AttractionBased");
        var points = _service.ProcessAttractionPoints(1, visitedAttraction.AttractionId, null);
        Assert.AreEqual(100, points);
    }

    [TestMethod]
    public void ProcessAttractionAccess_UpdatesVisitorPoints()
    {
        _visitor.VisitorParkInformation.Points = 250;
        var nfc = Guid.NewGuid();
        _visitor.VisitorParkInformation.NFC = nfc;
        var ticket = new Ticket();
        ticket.VisitorParkInformationId = _visitor.VisitorParkInformation.Id;

        _visitorService.Setup(vs => vs.GetAll(null)).Returns(new List<Visitor> { _visitor }.AsQueryable());

        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1
        };
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));
        _service.SetActiveStrategy("AttractionBased");
        _service.ProcessAttractionPoints(null, visitedAttraction.AttractionId, nfc.ToString());
        Assert.AreEqual(350, _visitor.VisitorParkInformation.Points);
    }

    [TestMethod]
    public void ProcessAttractionAccessWithNfc_UpdatesVisitorPoints()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1
        };
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));
        _service.SetActiveStrategy("AttractionBased");
        _service.ProcessAttractionPoints(1, visitedAttraction.AttractionId, null);
        Assert.AreEqual(100, _visitor.VisitorParkInformation.Points);
    }

    [TestMethod]
    public void ProcessAttractionAccess_ThrowsException_WhenVisitorNotFound()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1
        };
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(999)).Returns(((Visitor?)null)!);
        Assert.ThrowsException<ArgumentException>(() => _service.ProcessAttractionPoints(999, visitedAttraction.AttractionId, null));
    }

    [TestMethod]
    public void ProcessAttractionAccess_WithLastVisitedAttractionYesterday_ResetPoints()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1,
            VisitDate = DateTime.Parse("2024/01/01").AddDays(-1)
        };

        _visitor.VisitorParkInformation.TodayPoints = 100;
        _visitor.VisitorParkInformation.VisitedAttractions[0] = visitedAttraction;

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));
        _service.ProcessAttractionPoints(1, visitedAttraction.AttractionId, null);

        Assert.AreEqual(0, _visitor.VisitorParkInformation.TodayPoints);
    }

    [TestMethod]
    public void GetDailyRanking_ReturnsEmpty_WhenNoVisitors()
    {
        _visitorService.Setup(r => r.GetAll(null)).Returns([]);
        Assert.AreEqual(0, _service.GetDailyRanking().Count);
    }

    [TestMethod]
    public void GetDailyRanking_ReturnsOneEntry_WithOneVisitor()
    {
        _visitorService.Setup(r => r.GetAll(null)).Returns([_visitor]);
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        Assert.AreEqual(1, _service.GetDailyRanking().Count);
    }

    [TestMethod]
    public void GetDailyRanking_ReturnsTop10Only()
    {
        var visitors = new List<Visitor>();
        for(var i = 1; i <= 15; i++)
        {
            var v = new Visitor
            {
                Id = i,
                Name = $"V{i}",
                Email = $"v{i}@test.com",
                VisitorParkInformation = new VisitorParkInformation
                {
                    TodayPoints = i * 10,
                    Tickets = [new Ticket { Date = DateTime.Parse("2024/01/01") }],
                    VisitedAttractions = [new VisitedAttraction { VisitDate = DateTime.Parse("2024/01/01"), VisitorParkInformationId = i }]
                }
            };
            visitors.Add(v);
            _visitorService.Setup(r => r.GetVisitorWithParkInformation(i)).Returns(v);
        }

        _visitorService.Setup(r => r.GetAll(null)).Returns(visitors);
        Assert.AreEqual(10, _service.GetDailyRanking().Count);
    }

    [TestMethod]
    public void GetDailyRanking_OrdersByPointsDescending()
    {
        var visitor2 = new Visitor
        {
            Id = 2,
            Name = "Jane",
            Email = "jane@test.com",
            VisitorParkInformation = new VisitorParkInformation
            {
                TodayPoints = 200,
                Tickets = [new Ticket { Date = DateTime.Parse("2024/01/01") }],
                VisitedAttractions = [new VisitedAttraction { VisitDate = DateTime.Parse("2024/01/01"), VisitorParkInformationId = 2 }]
            }
        };
        _visitor.VisitorParkInformation.TodayPoints = 100;
        _visitor.VisitorParkInformation.VisitedAttractions[0] =
            new VisitedAttraction { VisitDate = DateTime.Parse("2024/01/01"), VisitorParkInformationId = 2 };

        _visitorService.Setup(r => r.GetAll(null)).Returns([_visitor, visitor2]);
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(2)).Returns(visitor2);
        var ranking = _service.GetDailyRanking();
        Assert.AreEqual(200, ranking.First().Value);
    }

    [TestMethod]
    public void GetDailyRanking_ResetTodayPoints()
    {
        _visitorService.Setup(r => r.GetAll(null)).Returns([_visitor]);
        _visitorService.Setup(vs => vs.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(vs => vs.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));

        _visitor.VisitorParkInformation.TodayPoints = 100;
        _visitor.VisitorParkInformation.VisitedAttractions[0] = new VisitedAttraction { VisitDate = DateTime.Now.AddYears(-10) };

        _service.GetDailyRanking();
        Assert.AreEqual(0, _visitor.VisitorParkInformation.TodayPoints);
    }

    [TestMethod]
    public void ProcessAttractionAccess_RecordsTransactionInHistory()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1
        };

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));

        _pointsHistoryService.Setup(phs => phs.RecordTransaction(
            1,
            100,
            "Attraction",
            "Completed attraction ID: 1",
            "AttractionBased",
            1)).Verifiable();

        _service.SetActiveStrategy("AttractionBased");

        var points = _service.ProcessAttractionPoints(1, visitedAttraction.AttractionId, null);

        Assert.AreEqual(100, points);
        _pointsHistoryService.Verify(phs => phs.RecordTransaction(
            1, 100, "Attraction", "Completed attraction ID: 1", "AttractionBased", 1),
            Times.Once);
    }

    [TestMethod]
    public void ProcessAttractionAccessWithNfc_RecordsTransactionInHistory()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1
        };

        var nfc = Guid.NewGuid();
        _visitor.VisitorParkInformation.NFC = nfc;

        _visitorService.Setup(vs => vs.GetAll(null)).Returns(new List<Visitor> { _visitor }.AsQueryable());
        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));

        _pointsHistoryService.Setup(phs => phs.RecordTransaction(
            1,
            100,
            "Attraction",
            "Completed attraction ID: 1",
            "AttractionBased",
            1)).Verifiable();

        _service.SetActiveStrategy("AttractionBased");

        var points = _service.ProcessAttractionPoints(null, visitedAttraction.AttractionId, nfc.ToString());

        Assert.AreEqual(100, points);
        _pointsHistoryService.Verify(phs => phs.RecordTransaction(
            1, 100, "Attraction", "Completed attraction ID: 1", "AttractionBased", 1),
            Times.Once);
    }

    [TestMethod]
    public void ProcessAttractionAccess_ComboStrategy_RecordsTransactionInHistory()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1,
            VisitDate = DateTime.Parse("2024/01/01 10:00"),
            VisitorParkInformationId = 1
        };

        var previousAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.Simulator,
            VisitDate = DateTime.Parse("2024/01/01 09:57"),
            AttractionId = 2,
            VisitorParkInformationId = 1
        };

        _visitor.VisitorParkInformation.VisitedAttractions = [previousAttraction, visitedAttraction];

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));

        _pointsHistoryService.Setup(phs => phs.RecordTransaction(
            1,
            100,
            "Attraction",
            "Completed attraction ID: 1",
            "Combo",
            1)).Verifiable();

        _service.SetActiveStrategy("Combo");

        var points = _service.ProcessAttractionPoints(1, visitedAttraction.AttractionId, null);

        Assert.AreEqual(100, points);
        _pointsHistoryService.Verify(phs => phs.RecordTransaction(
            1, 100, "Attraction", "Completed attraction ID: 1", "Combo", 1),
            Times.Once);
    }

    [TestMethod]
    public void ProcessAttractionAccess_EventStrategy_RecordsTransactionInHistory()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = true,
            AttractionId = 1,
            VisitDate = DateTime.Parse("2024/01/01 10:00"),
            VisitorParkInformationId = 1
        };

        _visitor.VisitorParkInformation.VisitedAttractions[0] = visitedAttraction;

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));

        _pointsHistoryService.Setup(phs => phs.RecordTransaction(
            1,
            100,
            "Attraction",
            "Completed attraction ID: 1",
            "Event",
            1)).Verifiable();

        _service.SetActiveStrategy("Event");

        var points = _service.ProcessAttractionPoints(1, visitedAttraction.AttractionId, null);

        Assert.AreEqual(100, points);
        _pointsHistoryService.Verify(phs => phs.RecordTransaction(
            1, 100, "Attraction", "Completed attraction ID: 1", "Event", 1),
            Times.Once);
    }

    [TestMethod]
    public void ProcessAttractionAccess_WithResetPoints_DoesNotRecordTransaction()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false,
            AttractionId = 1,
            VisitDate = DateTime.Parse("2024/01/01").AddDays(-1)
        };

        _visitor.VisitorParkInformation.TodayPoints = 100;
        _visitor.VisitorParkInformation.VisitedAttractions[0] = visitedAttraction;

        _visitorService.Setup(r => r.GetVisitorWithParkInformation(1)).Returns(_visitor);
        _visitorService.Setup(r => r.UpdateVisitorParkInformation(It.IsAny<VisitorParkInformation>()));

        var points = _service.ProcessAttractionPoints(1, visitedAttraction.AttractionId, null);

        Assert.AreEqual(0, points);
        _pointsHistoryService.Verify(phs => phs.RecordTransaction(
            It.IsAny<int>(),
            It.IsAny<int>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>()),
            Times.Never);
    }
}
