using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;

[TestClass]
public class PointsHistoryControllerTest
{
    private Mock<IPointsHistoryService> _mockPointsHistoryService = null!;
    private Mock<IVisitorService> _mockVisitorService = null!;
    private PointsHistoryController _controller = null!;
    private Visitor _testVisitor = null!;
    private List<PointsHistory> _testHistory = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockPointsHistoryService = new Mock<IPointsHistoryService>();
        _mockVisitorService = new Mock<IVisitorService>();
        _controller = new PointsHistoryController(_mockPointsHistoryService.Object, _mockVisitorService.Object);

        _testVisitor = new Visitor
        {
            Id = 1,
            VisitorParkInformation = new VisitorParkInformation
            {
                Id = 1,
                Points = 1500,
                TodayPoints = 100,
                RedeemPointsToday = 2
            }
        };

        _testHistory =
        [
            new PointsHistory { Id = 1, Points = 100, Origin = "Attraction" },
            new PointsHistory { Id = 2, Points = -50, Origin = "Reward" },
            new PointsHistory { Id = 3, Points = 200, Origin = "SpecialEvent" }
        ];
    }

    [TestMethod]
    public void Constructor_WithValidServices_CreatesController()
    {
        Assert.IsNotNull(_controller);
    }

    [TestMethod]
    public void GetVisitorHistory_WithExistingVisitor_ReturnsOkResult()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetVisitorHistory(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetVisitorHistory_WithExistingVisitor_ReturnsGetPointsHistoryResponse()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetVisitorHistory(1) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(GetPointsHistoryResponse));
    }

    [TestMethod]
    public void GetVisitorHistory_WithNonExistingVisitor_ReturnsNotFound()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);

        var result = _controller.GetVisitorHistory(1);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void GetVisitorHistory_WithNonExistingVisitor_ReturnsVisitorNotFoundMessage()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);

        var result = _controller.GetVisitorHistory(1) as NotFoundObjectResult;

        Assert.AreEqual("Visitor not found", result!.Value);
    }

    [TestMethod]
    public void GetVisitorHistory_CallsPointsHistoryServiceGetVisitorHistory()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.GetVisitorHistory(1);

        _mockPointsHistoryService.Verify(x => x.GetVisitorHistory(1), Times.Once);
    }

    [TestMethod]
    public void GetVisitorHistory_CallsVisitorServiceGetVisitorWithParkInformation()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.GetVisitorHistory(1);

        _mockVisitorService.Verify(x => x.GetVisitorWithParkInformation(1), Times.Once);
    }

    [TestMethod]
    public void GetHistoryByDateRange_WithValidRequest_ReturnsOkResult()
    {
        var request = new GetPointsHistoryRequest
        {
            VisitorId = 1,
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 12, 31)
        };
        _mockPointsHistoryService.Setup(x => x.GetHistoryByDateRange(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetHistoryByDateRange(request);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetHistoryByDateRange_WithValidRequest_ReturnsGetPointsHistoryResponse()
    {
        var request = new GetPointsHistoryRequest
        {
            VisitorId = 1,
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 12, 31)
        };
        _mockPointsHistoryService.Setup(x => x.GetHistoryByDateRange(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetHistoryByDateRange(request) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(GetPointsHistoryResponse));
    }

    [TestMethod]
    public void GetHistoryByDateRange_WithNullDates_UsesDefaultDates()
    {
        var request = new GetPointsHistoryRequest
        {
            VisitorId = 1,
            StartDate = null,
            EndDate = null
        };
        _mockPointsHistoryService.Setup(x => x.GetHistoryByDateRange(1, DateTime.MinValue, It.IsAny<DateTime>())).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.GetHistoryByDateRange(request);

        _mockPointsHistoryService.Verify(x => x.GetHistoryByDateRange(1, DateTime.MinValue, It.IsAny<DateTime>()), Times.Once);
    }

    [TestMethod]
    public void GetHistoryByDateRange_WithStartDateOnly_UsesDefaultEndDate()
    {
        var request = new GetPointsHistoryRequest
        {
            VisitorId = 1,
            StartDate = new DateTime(2025, 1, 1),
            EndDate = null
        };
        _mockPointsHistoryService.Setup(x => x.GetHistoryByDateRange(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.GetHistoryByDateRange(request);

        _mockPointsHistoryService.Verify(x => x.GetHistoryByDateRange(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
    }

    [TestMethod]
    public void GetHistoryByDateRange_WithNonExistingVisitor_ReturnsNotFound()
    {
        var request = new GetPointsHistoryRequest { VisitorId = 999 };
        _mockPointsHistoryService.Setup(x => x.GetHistoryByDateRange(999, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(999)).Returns((Visitor)null!);

        var result = _controller.GetHistoryByDateRange(request);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void GetHistoryByDateRange_CallsPointsHistoryServiceGetHistoryByDateRange()
    {
        var request = new GetPointsHistoryRequest
        {
            VisitorId = 1,
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2025, 12, 31)
        };
        _mockPointsHistoryService.Setup(x => x.GetHistoryByDateRange(1, It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.GetHistoryByDateRange(request);

        _mockPointsHistoryService.Verify(x => x.GetHistoryByDateRange(1, It.IsAny<DateTime>(), It.IsAny<DateTime>()), Times.Once);
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_ReturnsOkResult()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_ReturnsPointsSummaryResponse()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(PointsSummaryResponse));
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_SetsCorrectVisitorId()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(1, response!.VisitorId);
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_CalculatesTotalPointsEarnedCorrectly()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(300, response!.TotalPointsEarned); // 100 + 200
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_CalculatesTotalPointsSpentCorrectly()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(50, response!.TotalPointsSpent); // Math.Abs(-50)
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_SetsCurrentBalanceCorrectly()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(1500, response!.CurrentBalance);
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_SetsTodayPointsCorrectly()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(100, response!.TodayPoints);
    }

    [TestMethod]
    public void GetPointsSummary_WithExistingVisitor_SetsRedemptionsTodayCorrectly()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(2, response!.RedemptionsToday);
    }

    [TestMethod]
    public void GetPointsSummary_WithOnlyPositivePoints_CalculatesTotalsCorrectly()
    {
        var positiveHistory = new List<PointsHistory>
        {
            new PointsHistory { Points = 100 },
            new PointsHistory { Points = 200 }
        };
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(positiveHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(300, response!.TotalPointsEarned);
        Assert.AreEqual(0, response!.TotalPointsSpent);
    }

    [TestMethod]
    public void GetPointsSummary_WithOnlyNegativePoints_CalculatesTotalsCorrectly()
    {
        var negativeHistory = new List<PointsHistory>
        {
            new PointsHistory { Points = -50 },
            new PointsHistory { Points = -100 }
        };
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(negativeHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(0, response!.TotalPointsEarned);
        Assert.AreEqual(150, response!.TotalPointsSpent);
    }

    [TestMethod]
    public void GetPointsSummary_WithEmptyHistory_CalculatesZeroTotals()
    {
        var emptyHistory = new List<PointsHistory>();
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(emptyHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetPointsSummary(1) as OkObjectResult;
        var response = result!.Value as PointsSummaryResponse;

        Assert.AreEqual(0, response!.TotalPointsEarned);
        Assert.AreEqual(0, response!.TotalPointsSpent);
    }

    [TestMethod]
    public void GetPointsSummary_WithNonExistingVisitor_ReturnsNotFound()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(999)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(999)).Returns((Visitor)null!);

        var result = _controller.GetPointsSummary(999);

        Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public void GetPointsSummary_CallsPointsHistoryServiceGetVisitorHistory()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.GetPointsSummary(1);

        _mockPointsHistoryService.Verify(x => x.GetVisitorHistory(1), Times.Once);
    }

    [TestMethod]
    public void GetPointsSummary_CallsVisitorServiceGetVisitorWithParkInformation()
    {
        _mockPointsHistoryService.Setup(x => x.GetVisitorHistory(1)).Returns(_testHistory);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.GetPointsSummary(1);

        _mockVisitorService.Verify(x => x.GetVisitorWithParkInformation(1), Times.Once);
    }
}
