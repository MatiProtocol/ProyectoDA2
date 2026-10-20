using Abstraction;
using Domain;
using Moq;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class PointsHistoryServiceTest
{
    private Mock<IPointsHistoryRepository> _mockPointsHistoryRepository = null!;
    private Mock<IVisitorService> _mockVisitorService = null!;
    private IPointsHistoryService _pointsHistoryService = null!;
    private Visitor _testVisitor = null!;
    private PointsHistory _testPointsHistory = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockPointsHistoryRepository = new Mock<IPointsHistoryRepository>();
        _mockVisitorService = new Mock<IVisitorService>();
        _pointsHistoryService = new PointsHistoryService(
            _mockPointsHistoryRepository.Object,
            _mockVisitorService.Object);

        _testVisitor = new Visitor
        {
            Id = 1,
            VisitorParkInformation = new VisitorParkInformation
            {
                Id = 1,
                Points = 2000
            }
        };

        _testPointsHistory = new PointsHistory
        {
            Id = 1,
            VisitorParkInformationId = 1,
            Points = 100,
            Origin = "Attraction",
            Description = "Completed roller coaster",
            ActiveStrategy = "AttractionBased",
            RelatedEntityId = 1,
            TransactionDate = new DateTime(2025, 10, 15, 10, 30, 0)
        };
    }

    [TestMethod]
    public void CreatePointsHistoryService_IsNotNull()
    {
        Assert.IsNotNull(_pointsHistoryService);
    }

    [TestMethod]
    public void RecordTransaction_CallsRepositoryAdd()
    {
        _pointsHistoryService.RecordTransaction(1, 100, "Attraction", "Test", "Strategy", 1);
        _mockPointsHistoryRepository.Verify(x => x.Add(It.IsAny<PointsHistory>()), Times.Once);
    }

    [TestMethod]
    public void RecordTransaction_WithNullRelatedEntityId_CallsRepositoryAdd()
    {
        _pointsHistoryService.RecordTransaction(1, 100, "Attraction", "Test", "Strategy", null);
        _mockPointsHistoryRepository.Verify(x => x.Add(It.IsAny<PointsHistory>()), Times.Once);
    }

    [TestMethod]
    public void RecordTransaction_WithNegativePoints_CallsRepositoryAdd()
    {
        _pointsHistoryService.RecordTransaction(1, -500, "Reward", "Redeemed", "Strategy", 2);
        _mockPointsHistoryRepository.Verify(x => x.Add(It.IsAny<PointsHistory>()), Times.Once);
    }

    [TestMethod]
    public void RecordTransaction_SetsCorrectVisitorParkInformationId()
    {
        PointsHistory capturedHistory = null!;
        _mockPointsHistoryRepository.Setup(x => x.Add(It.IsAny<PointsHistory>()))
            .Callback<PointsHistory>(history => capturedHistory = history);

        _pointsHistoryService.RecordTransaction(1, 100, "Origin", "Desc", "Strategy", 1);

        Assert.AreEqual(1, capturedHistory.VisitorParkInformationId);
    }

    [TestMethod]
    public void RecordTransaction_SetsCorrectPoints()
    {
        PointsHistory capturedHistory = null!;
        _mockPointsHistoryRepository.Setup(x => x.Add(It.IsAny<PointsHistory>()))
            .Callback<PointsHistory>(history => capturedHistory = history);

        _pointsHistoryService.RecordTransaction(1, 150, "Origin", "Desc", "Strategy", 1);

        Assert.AreEqual(150, capturedHistory.Points);
    }

    [TestMethod]
    public void RecordTransaction_SetsCorrectOrigin()
    {
        PointsHistory capturedHistory = null!;
        _mockPointsHistoryRepository.Setup(x => x.Add(It.IsAny<PointsHistory>()))
            .Callback<PointsHistory>(history => capturedHistory = history);

        _pointsHistoryService.RecordTransaction(1, 100, "TestOrigin", "Desc", "Strategy", 1);

        Assert.AreEqual("TestOrigin", capturedHistory.Origin);
    }

    [TestMethod]
    public void RecordTransaction_SetsCorrectDescription()
    {
        PointsHistory capturedHistory = null!;
        _mockPointsHistoryRepository.Setup(x => x.Add(It.IsAny<PointsHistory>()))
            .Callback<PointsHistory>(history => capturedHistory = history);

        _pointsHistoryService.RecordTransaction(1, 100, "Origin", "TestDescription", "Strategy", 1);

        Assert.AreEqual("TestDescription", capturedHistory.Description);
    }

    [TestMethod]
    public void RecordTransaction_SetsCorrectActiveStrategy()
    {
        PointsHistory capturedHistory = null!;
        _mockPointsHistoryRepository.Setup(x => x.Add(It.IsAny<PointsHistory>()))
            .Callback<PointsHistory>(history => capturedHistory = history);

        _pointsHistoryService.RecordTransaction(1, 100, "Origin", "Desc", "TestStrategy", 1);

        Assert.AreEqual("TestStrategy", capturedHistory.ActiveStrategy);
    }

    [TestMethod]
    public void RecordTransaction_SetsCorrectRelatedEntityId()
    {
        PointsHistory capturedHistory = null!;
        _mockPointsHistoryRepository.Setup(x => x.Add(It.IsAny<PointsHistory>()))
            .Callback<PointsHistory>(history => capturedHistory = history);

        _pointsHistoryService.RecordTransaction(1, 100, "Origin", "Desc", "Strategy", 5);

        Assert.AreEqual(5, capturedHistory.RelatedEntityId);
    }

    [TestMethod]
    public void RecordTransaction_SetsTransactionDate()
    {
        PointsHistory capturedHistory = null!;
        _mockPointsHistoryRepository.Setup(x => x.Add(It.IsAny<PointsHistory>()))
            .Callback<PointsHistory>(history => capturedHistory = history);

        _pointsHistoryService.RecordTransaction(1, 100, "Origin", "Desc", "Strategy", 1);

        Assert.IsTrue(capturedHistory.TransactionDate <= DateTime.Now);
    }

    [TestMethod]
    public void GetVisitorHistory_ReturnsHistory()
    {
        var expectedHistory = new List<PointsHistory> { _testPointsHistory };
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByVisitorParkInfoId(1)).Returns(expectedHistory);

        var result = _pointsHistoryService.GetVisitorHistory(1);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetVisitorHistory_WithNonExistingVisitor_ThrowsException()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        _pointsHistoryService.GetVisitorHistory(1);
    }

    [TestMethod]
    public void GetVisitorHistory_WithEmptyHistory_ReturnsEmptyList()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByVisitorParkInfoId(1)).Returns([]);

        var result = _pointsHistoryService.GetVisitorHistory(1);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetVisitorHistory_CallsRepositoryWithCorrectId()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByVisitorParkInfoId(1)).Returns([]);

        _pointsHistoryService.GetVisitorHistory(1);

        _mockPointsHistoryRepository.Verify(x => x.GetByVisitorParkInfoId(1), Times.Once);
    }

    [TestMethod]
    public void GetHistoryByDateRange_ReturnsFilteredHistory()
    {
        var startDate = new DateTime(2025, 10, 1);
        var endDate = new DateTime(2025, 10, 31);
        var expectedHistory = new List<PointsHistory> { _testPointsHistory };

        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByDateRange(1, startDate, endDate)).Returns(expectedHistory);

        var result = _pointsHistoryService.GetHistoryByDateRange(1, startDate, endDate);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetHistoryByDateRange_WithNonExistingVisitor_ThrowsException()
    {
        var startDate = new DateTime(2025, 10, 1);
        var endDate = new DateTime(2025, 10, 31);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);

        _pointsHistoryService.GetHistoryByDateRange(1, startDate, endDate);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GetHistoryByDateRange_WithStartDateGreaterThanEndDate_ThrowsException()
    {
        var startDate = new DateTime(2025, 10, 31);
        var endDate = new DateTime(2025, 10, 1);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _pointsHistoryService.GetHistoryByDateRange(1, startDate, endDate);
    }

    [TestMethod]
    public void GetHistoryByDateRange_WithSameStartAndEndDate_ReturnsHistory()
    {
        var startDate = new DateTime(2025, 10, 15);
        var endDate = new DateTime(2025, 10, 15);
        var expectedHistory = new List<PointsHistory> { _testPointsHistory };

        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByDateRange(1, startDate, endDate)).Returns(expectedHistory);

        var result = _pointsHistoryService.GetHistoryByDateRange(1, startDate, endDate);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetHistoryByDateRange_WithNoMatchingRecords_ReturnsEmptyList()
    {
        var startDate = new DateTime(2025, 11, 1);
        var endDate = new DateTime(2025, 11, 30);

        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByDateRange(1, startDate, endDate)).Returns([]);

        var result = _pointsHistoryService.GetHistoryByDateRange(1, startDate, endDate);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetHistoryByDateRange_CallsRepositoryWithCorrectParameters()
    {
        var startDate = new DateTime(2025, 10, 1);
        var endDate = new DateTime(2025, 10, 31);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByDateRange(1, startDate, endDate)).Returns([]);

        _pointsHistoryService.GetHistoryByDateRange(1, startDate, endDate);

        _mockPointsHistoryRepository.Verify(x => x.GetByDateRange(1, startDate, endDate), Times.Once);
    }

    [TestMethod]
    public void GetVisitorHistory_ReturnsCorrectHistoryItem()
    {
        var expectedHistory = new List<PointsHistory> { _testPointsHistory };
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByVisitorParkInfoId(1)).Returns(expectedHistory);

        var result = _pointsHistoryService.GetVisitorHistory(1);

        Assert.AreEqual(_testPointsHistory, result[0]);
    }

    [TestMethod]
    public void RecordTransaction_WithZeroPoints_CallsRepositoryAdd()
    {
        _pointsHistoryService.RecordTransaction(1, 0, "Origin", "Desc", "Strategy", 1);
        _mockPointsHistoryRepository.Verify(x => x.Add(It.IsAny<PointsHistory>()), Times.Once);
    }

    [TestMethod]
    public void GetHistoryByDateRange_ReturnsCorrectHistoryItems()
    {
        var startDate = new DateTime(2025, 10, 1);
        var endDate = new DateTime(2025, 10, 31);
        var expectedHistory = new List<PointsHistory> { _testPointsHistory };

        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockPointsHistoryRepository.Setup(x => x.GetByDateRange(1, startDate, endDate)).Returns(expectedHistory);

        var result = _pointsHistoryService.GetHistoryByDateRange(1, startDate, endDate);

        Assert.AreEqual(_testPointsHistory, result[0]);
    }
}
