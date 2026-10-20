using Abstraction;
using DataAccess;
using Domain;

namespace Tests.RepositoriesTest;

[TestClass]
public class PointsHistoryRepositoryTest
{
    private SqlContext _context = null!;
    private IPointsHistoryRepository _repository = null!;
    private VisitorParkInformation _visitorParkInfo = null!;
    private PointsHistory _pointsHistory = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _visitorParkInfo = new VisitorParkInformation
        {
            Id = 1,
            Points = 5000,
            VisitorMembership = "Premium"
        };

        _context.VisitorParkInformations.Add(_visitorParkInfo);
        _context.SaveChanges();

        _pointsHistory = new PointsHistory
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            Points = 100,
            Origin = "Attraction",
            Description = "Visited roller coaster",
            ActiveStrategy = "AttractionBased",
            TransactionDate = DateTime.Now
        };

        _context.PointsHistory.Add(_pointsHistory);
        _context.SaveChanges();

        _repository = new PointsHistoryRepository(_context);
    }

    [TestCleanup]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [TestMethod]
    public void CreatePointsHistoryRepository_IsNotNull()
    {
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public void GetByVisitorParkInfoId_ReturnsHistory()
    {
        var result = _repository.GetByVisitorParkInfoId(_visitorParkInfo.Id);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetByVisitorParkInfoId_WithNoHistory_ReturnsEmptyList()
    {
        var result = _repository.GetByVisitorParkInfoId(999);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetByVisitorParkInfoId_OrdersByDateDescending()
    {
        var olderHistory = new PointsHistory
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            Points = 50,
            Origin = "Event",
            Description = "Special event bonus",
            ActiveStrategy = "EventBased",
            TransactionDate = DateTime.Now.AddDays(-5)
        };
        _context.PointsHistory.Add(olderHistory);
        _context.SaveChanges();

        var result = _repository.GetByVisitorParkInfoId(_visitorParkInfo.Id);

        Assert.IsTrue(result[0].TransactionDate > result[1].TransactionDate);
    }

    [TestMethod]
    public void GetByDateRange_ReturnsHistoryInRange()
    {
        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now.AddDays(1);

        var result = _repository.GetByDateRange(_visitorParkInfo.Id, startDate, endDate);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetByDateRange_WithNoHistoryInRange_ReturnsEmptyList()
    {
        var startDate = DateTime.Now.AddDays(-10);
        var endDate = DateTime.Now.AddDays(-5);

        var result = _repository.GetByDateRange(_visitorParkInfo.Id, startDate, endDate);

        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetByDateRange_WithMultipleEntries_ReturnsOnlyInRange()
    {
        var oldHistory = new PointsHistory
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            Points = 75,
            Origin = "Reward",
            Description = "Redeemed reward",
            ActiveStrategy = "RewardBased",
            TransactionDate = DateTime.Now.AddDays(-20)
        };
        _context.PointsHistory.Add(oldHistory);
        _context.SaveChanges();

        var startDate = DateTime.Now.AddDays(-1);
        var endDate = DateTime.Now.AddDays(1);

        var result = _repository.GetByDateRange(_visitorParkInfo.Id, startDate, endDate);

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetTotalPointsByVisitor_ReturnsSumOfPoints()
    {
        var additionalHistory = new PointsHistory
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            Points = 200,
            Origin = "Event",
            Description = "Event participation",
            ActiveStrategy = "EventBased",
            TransactionDate = DateTime.Now
        };
        _context.PointsHistory.Add(additionalHistory);
        _context.SaveChanges();

        var result = _repository.GetTotalPointsByVisitor(_visitorParkInfo.Id);

        Assert.AreEqual(300, result);
    }

    [TestMethod]
    public void GetTotalPointsByVisitor_WithNegativePoints_CalculatesCorrectly()
    {
        var negativeHistory = new PointsHistory
        {
            VisitorParkInformationId = _visitorParkInfo.Id,
            Points = -50,
            Origin = "Reward",
            Description = "Redeemed reward",
            ActiveStrategy = "RewardBased",
            TransactionDate = DateTime.Now
        };
        _context.PointsHistory.Add(negativeHistory);
        _context.SaveChanges();

        var result = _repository.GetTotalPointsByVisitor(_visitorParkInfo.Id);

        Assert.AreEqual(50, result);
    }

    [TestMethod]
    public void GetTotalPointsByVisitor_WithNoHistory_ReturnsZero()
    {
        var result = _repository.GetTotalPointsByVisitor(999);

        Assert.AreEqual(0, result);
    }
}
