using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NonDomainAbstractions;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;

[TestClass]
public class PunctuationsControllerTest
{
    private Mock<IPunctuationService> _mockService = null!;
    private Mock<IDateTimeService> _mockDateTimeService = null!;
    private PunctuationsController _controller = null!;
    private SetStrategyRequest _setStrategyRequest = null!;
    private ProcessAttractionAccessRequest _processAccessRequest = null!;
    private AddPointsRequest _addPointsRequest = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockService = new Mock<IPunctuationService>(MockBehavior.Strict);
        _mockDateTimeService = new Mock<IDateTimeService>(MockBehavior.Strict);
        _controller = new PunctuationsController(_mockService.Object, _mockDateTimeService.Object);

        _setStrategyRequest = new SetStrategyRequest
        {
            StrategyName = "AttractionBased"
        };

        _processAccessRequest = new ProcessAttractionAccessRequest
        {
            VisitorId = 1,
            AttractionId = 1,
        };

        _addPointsRequest = new AddPointsRequest
        {
            VisitorId = 1,
            Points = 50
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        _mockService.VerifyAll();
        _mockDateTimeService.VerifyAll();
    }

    [TestMethod]
    public void Constructor_WithValidServices_CreatesController()
    {
        Assert.IsNotNull(_controller);
    }

    [TestMethod]
    public void SetStrategy_WithValidRequest_ReturnsOkResult()
    {
        _mockService.Setup(s => s.SetActiveStrategy(_setStrategyRequest.StrategyName));

        var result = _controller.SetStrategy(_setStrategyRequest);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void SetStrategy_WithValidRequest_CallsServiceSetActiveStrategy()
    {
        _mockService.Setup(s => s.SetActiveStrategy(_setStrategyRequest.StrategyName));

        _controller.SetStrategy(_setStrategyRequest);

        _mockService.Verify(s => s.SetActiveStrategy(_setStrategyRequest.StrategyName), Times.Once);
    }

    [TestMethod]
    public void GetActiveStrategy_ReturnsOkResult()
    {
        _mockService.Setup(s => s.GetActiveStrategy()).Returns("AttractionBased");

        var result = _controller.GetActiveStrategy();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetActiveStrategy_ReturnsCorrectStrategyName()
    {
        _mockService.Setup(s => s.GetActiveStrategy()).Returns("AttractionBased");

        var result = _controller.GetActiveStrategy() as OkObjectResult;
        var response = result?.Value as GetActiveStrategyResponse;

        Assert.AreEqual("AttractionBased", response?.StrategyName);
    }

    [TestMethod]
    public void GetAvailableStrategies_ReturnsOkResult()
    {
        var strategies = new List<string> { "AttractionBased", "Combo", "Event" };
        _mockService.Setup(s => s.GetAvailableStrategies()).Returns(strategies);

        var result = _controller.GetAvailableStrategies();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetAvailableStrategies_ReturnsCorrectCount()
    {
        var strategies = new List<string> { "AttractionBased", "Combo", "Event" };
        _mockService.Setup(s => s.GetAvailableStrategies()).Returns(strategies);

        var result = _controller.GetAvailableStrategies() as OkObjectResult;
        var response = result?.Value as GetAvailableStrategiesResponse;

        Assert.AreEqual(3, response?.Strategies.Count);
    }

    [TestMethod]
    public void GetAvailableStrategies_ReturnsAllStrategies()
    {
        var strategies = new List<string> { "AttractionBased", "Combo", "Event" };
        _mockService.Setup(s => s.GetAvailableStrategies()).Returns(strategies);

        var result = _controller.GetAvailableStrategies() as OkObjectResult;
        var response = result?.Value as GetAvailableStrategiesResponse;

        Assert.IsTrue(response?.Strategies.Contains("AttractionBased"));
    }

    [TestMethod]
    public void GetVisitorPoints_WithValidId_ReturnsOkResult()
    {
        _mockService.Setup(s => s.GetVisitorTotalPoints(1, null)).Returns(100);

        var result = _controller.GetVisitorPoints(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetVisitorPoints_WithValidId_ReturnsCorrectPoints()
    {
        _mockService.Setup(s => s.GetVisitorTotalPoints(1, null)).Returns(100);

        var result = _controller.GetVisitorPoints(1) as OkObjectResult;
        var response = result?.Value as GetVisitorPointsResponse;

        Assert.AreEqual(100, response?.TotalPoints);
    }

    [TestMethod]
    public void GetVisitorPoints_WithValidId_ReturnsCorrectVisitorId()
    {
        _mockService.Setup(s => s.GetVisitorTotalPoints(1, null)).Returns(100);

        var result = _controller.GetVisitorPoints(1) as OkObjectResult;
        var response = result?.Value as GetVisitorPointsResponse;

        Assert.AreEqual(1, response?.VisitorId);
    }

    [TestMethod]
    public void GetDailyRanking_ReturnsOkResult()
    {
        var ranking = new Dictionary<string, int>
        {
            { "John Doe - john@email.com", 150 },
            { "Jane Smith - jane@email.com", 120 }
        };
        var today = new DateTime(2025, 10, 2);
        _mockService.Setup(s => s.GetDailyRanking()).Returns(ranking);
        _mockDateTimeService.Setup(d => d.Now).Returns(today);
        var result = _controller.GetDailyRanking();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetDailyRanking_ReturnsCorrectCount()
    {
        var ranking = new Dictionary<string, int>
        {
            { "John Doe - john@email.com", 150 },
            { "Jane Smith - jane@email.com", 120 }
        };
        var today = new DateTime(2025, 10, 2);
        _mockService.Setup(s => s.GetDailyRanking()).Returns(ranking);
        _mockDateTimeService.Setup(d => d.Now).Returns(today);

        var result = _controller.GetDailyRanking() as OkObjectResult;
        var response = result?.Value as GetDailyRankingResponse;

        Assert.AreEqual(2, response?.Ranking.Count);
    }

    [TestMethod]
    public void GetDailyRanking_ReturnsCorrectDate()
    {
        var ranking = new Dictionary<string, int>
        {
            { "John Doe - john@email.com", 150 }
        };
        var today = new DateTime(2025, 10, 2);
        _mockService.Setup(s => s.GetDailyRanking()).Returns(ranking);
        _mockDateTimeService.Setup(d => d.Now).Returns(today);

        var result = _controller.GetDailyRanking() as OkObjectResult;
        var response = result?.Value as GetDailyRankingResponse;

        Assert.AreEqual(today.Date, response?.RankingDate);
    }

    [TestMethod]
    public void GetDailyRanking_ReturnsCorrectPositions()
    {
        var ranking = new Dictionary<string, int>
        {
            { "John Doe - john@email.com", 150 },
            { "Jane Smith - jane@email.com", 120 }
        };
        var today = new DateTime(2025, 10, 2);
        _mockService.Setup(s => s.GetDailyRanking()).Returns(ranking);
        _mockDateTimeService.Setup(d => d.Now).Returns(today);

        var result = _controller.GetDailyRanking() as OkObjectResult;
        var response = result?.Value as GetDailyRankingResponse;

        Assert.AreEqual(1, response?.Ranking[0].Position);
    }

    [TestMethod]
    public void GetDailyRanking_WithEmptyRanking_ReturnsEmptyList()
    {
        var ranking = new Dictionary<string, int>();
        var today = new DateTime(2025, 10, 2);
        _mockService.Setup(s => s.GetDailyRanking()).Returns(ranking);
        _mockDateTimeService.Setup(d => d.Now).Returns(today);

        var result = _controller.GetDailyRanking() as OkObjectResult;
        var response = result?.Value as GetDailyRankingResponse;

        Assert.AreEqual(0, response?.Ranking.Count);
    }

    [TestMethod]
    public void ProcessAttractionAccess_WithValidRequest_ReturnsOkResult()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false
        };
        _mockService.Setup(s => s.ProcessAttractionPoints(
            _processAccessRequest.VisitorId, _processAccessRequest.AttractionId, null)).Returns(100);
        _mockService.Setup(s => s.GetVisitorTotalPoints(_processAccessRequest.VisitorId, null)).Returns(250);

        var result = _controller.ProcessAttractionAccess(_processAccessRequest);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void ProcessAttractionAccess_WithValidRequest_ReturnsCorrectPointsAwarded()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false
        };
        _mockService.Setup(s => s.ProcessAttractionPoints(
            _processAccessRequest.VisitorId, _processAccessRequest.AttractionId, null)).Returns(100);
        _mockService.Setup(s => s.GetVisitorTotalPoints(_processAccessRequest.VisitorId, null)).Returns(250);

        var result = _controller.ProcessAttractionAccess(_processAccessRequest) as OkObjectResult;
        var response = result?.Value as ProcessAccessResponse;

        Assert.AreEqual(100, response?.PointsAwarded);
    }

    [TestMethod]
    public void ProcessAttractionAccess_WithValidRequest_ReturnsCorrectTotalPoints()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false
        };
        _mockService.Setup(s => s.ProcessAttractionPoints(
            _processAccessRequest.VisitorId, _processAccessRequest.AttractionId, null)).Returns(100);
        _mockService.Setup(s => s.GetVisitorTotalPoints(_processAccessRequest.VisitorId, null)).Returns(250);

        var result = _controller.ProcessAttractionAccess(_processAccessRequest) as OkObjectResult;
        var response = result?.Value as ProcessAccessResponse;

        Assert.AreEqual(250, response?.TotalPoints);
    }

    [TestMethod]
    public void ProcessAttractionAccess_WithValidRequest_ReturnsCorrectVisitorId()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false
        };
        _mockService.Setup(s => s.ProcessAttractionPoints(
            _processAccessRequest.VisitorId, _processAccessRequest.AttractionId, null)).Returns(100);
        _mockService.Setup(s => s.GetVisitorTotalPoints(_processAccessRequest.VisitorId, null)).Returns(250);

        var result = _controller.ProcessAttractionAccess(_processAccessRequest) as OkObjectResult;
        var response = result?.Value as ProcessAccessResponse;

        Assert.AreEqual(1, response?.VisitorId);
    }

    [TestMethod]
    public void ProcessAttractionAccess_WithSpecialEvent_CallsServiceWithCorrectParameters()
    {
        var specialEventRequest = new ProcessAttractionAccessRequest
        {
            VisitorId = 1,
            AttractionId = 1
        };
        _mockService.Setup(s => s.ProcessAttractionPoints(1, _processAccessRequest.AttractionId, null)).Returns(120);
        _mockService.Setup(s => s.GetVisitorTotalPoints(1, null)).Returns(300);

        _controller.ProcessAttractionAccess(specialEventRequest);

        _mockService.Verify(s => s.ProcessAttractionPoints(1, _processAccessRequest.AttractionId, null), Times.Once);
    }

    [TestMethod]
    public void GetActiveStrategyResponse_Constructor_SetsStrategyName()
    {
        var response = new GetActiveStrategyResponse("Combo");

        Assert.AreEqual("Combo", response.StrategyName);
    }

    [TestMethod]
    public void GetAvailableStrategiesResponse_Constructor_SetsStrategies()
    {
        var strategies = new List<string> { "Strategy1", "Strategy2" };

        var response = new GetAvailableStrategiesResponse(strategies);

        Assert.AreEqual(2, response.Strategies.Count);
    }

    [TestMethod]
    public void GetVisitorPointsResponse_Constructor_SetsVisitorId()
    {
        var response = new GetVisitorPointsResponse(5, 200);

        Assert.AreEqual(5, response.VisitorId);
    }

    [TestMethod]
    public void GetVisitorPointsResponse_Constructor_SetsTotalPoints()
    {
        var response = new GetVisitorPointsResponse(5, 200);

        Assert.AreEqual(200, response.TotalPoints);
    }

    [TestMethod]
    public void ProcessAccessResponse_Constructor_SetsVisitorId()
    {
        var response = new ProcessAccessResponse(3, 80, 280);

        Assert.AreEqual(3, response.VisitorId);
    }

    [TestMethod]
    public void ProcessAccessResponse_Constructor_SetsPointsAwarded()
    {
        var response = new ProcessAccessResponse(3, 80, 280);

        Assert.AreEqual(80, response.PointsAwarded);
    }

    [TestMethod]
    public void ProcessAccessResponse_Constructor_SetsTotalPoints()
    {
        var response = new ProcessAccessResponse(3, 80, 280);

        Assert.AreEqual(280, response.TotalPoints);
    }

    [TestMethod]
    public void RankingEntryResponse_Constructor_SetsVisitorName()
    {
        var response = new RankingEntryResponse("Test User", 150, 1);

        Assert.AreEqual("Test User", response.VisitorName);
    }

    [TestMethod]
    public void RankingEntryResponse_Constructor_SetsPoints()
    {
        var response = new RankingEntryResponse("Test User", 150, 1);

        Assert.AreEqual(150, response.Points);
    }

    [TestMethod]
    public void RankingEntryResponse_Constructor_SetsPosition()
    {
        var response = new RankingEntryResponse("Test User", 150, 1);

        Assert.AreEqual(1, response.Position);
    }

    [TestMethod]
    public void GetDailyRankingResponse_Constructor_SetsRankingDate()
    {
        var ranking = new Dictionary<string, int> { { "User1", 100 } };
        var date = new DateTime(2025, 10, 2);

        var response = new GetDailyRankingResponse(ranking, date);

        Assert.AreEqual(date, response.RankingDate);
    }

    [TestMethod]
    public void GetDailyRankingResponse_Constructor_CreatesRankingList()
    {
        var ranking = new Dictionary<string, int>
        {
            { "User1", 100 },
            { "User2", 80 }
        };
        var date = new DateTime(2025, 10, 2);

        var response = new GetDailyRankingResponse(ranking, date);

        Assert.AreEqual(2, response.Ranking.Count);
    }

    [TestMethod]
    public void GetDailyRankingResponse_Constructor_AssignsSequentialPositions()
    {
        var ranking = new Dictionary<string, int>
        {
            { "User1", 100 },
            { "User2", 80 },
            { "User3", 60 }
        };
        var date = new DateTime(2025, 10, 2);

        var response = new GetDailyRankingResponse(ranking, date);

        Assert.AreEqual(2, response.Ranking[1].Position);
    }

    [TestMethod]
    public void GetDailyRanking_CallsServiceGetDailyRanking()
    {
        var ranking = new Dictionary<string, int> { { "User", 100 } };
        var today = new DateTime(2025, 10, 2);
        _mockService.Setup(s => s.GetDailyRanking()).Returns(ranking);
        _mockDateTimeService.Setup(d => d.Now).Returns(today);

        _controller.GetDailyRanking();

        _mockService.Verify(s => s.GetDailyRanking(), Times.Once);
    }

    [TestMethod]
    public void ProcessAttractionAccess_CallsGetVisitorTotalPoints()
    {
        var visitedAttraction = new VisitedAttraction
        {
            AttractionType = TypeAttraction.RollerCoaster,
            IsSpecialEvent = false
        };
        _mockService.Setup(s => s.ProcessAttractionPoints(
            _processAccessRequest.VisitorId, _processAccessRequest.AttractionId, null)).Returns(100);
        _mockService.Setup(s => s.GetVisitorTotalPoints(_processAccessRequest.VisitorId, null)).Returns(250);

        _controller.ProcessAttractionAccess(_processAccessRequest);

        _mockService.Verify(s => s.GetVisitorTotalPoints(_processAccessRequest.VisitorId, null), Times.Once);
    }

    [TestMethod]
    public void GetVisitorPoints_CallsServiceGetVisitorTotalPoints()
    {
        _mockService.Setup(s => s.GetVisitorTotalPoints(1, null)).Returns(100);

        _controller.GetVisitorPoints(1);

        _mockService.Verify(s => s.GetVisitorTotalPoints(1, null), Times.Once);
    }

    [TestMethod]
    public void GetActiveStrategy_CallsServiceGetActiveStrategy()
    {
        _mockService.Setup(s => s.GetActiveStrategy()).Returns("AttractionBased");

        _controller.GetActiveStrategy();

        _mockService.Verify(s => s.GetActiveStrategy(), Times.Once);
    }

    [TestMethod]
    public void GetAvailableStrategies_CallsServiceGetAvailableStrategies()
    {
        var strategies = new List<string> { "AttractionBased" };
        _mockService.Setup(s => s.GetAvailableStrategies()).Returns(strategies);

        _controller.GetAvailableStrategies();

        _mockService.Verify(s => s.GetAvailableStrategies(), Times.Once);
    }
}
