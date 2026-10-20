using Domain;

namespace Tests.DomainTest;

[TestClass]
public class PointsHistoryTest
{
    private PointsHistory _pointsHistory = null!;
    private DateTime _testDate;

    [TestInitialize]
    public void Initialize()
    {
        _testDate = new DateTime(2025, 10, 15, 10, 30, 0);
        _pointsHistory = new PointsHistory
        {
            Id = 1,
            VisitorParkInformationId = 5,
            TransactionDate = _testDate,
            Points = 100,
            Origin = "Attraction",
            Description = "Visited Dragon Coaster",
            ActiveStrategy = "AttractionBased",
            RelatedEntityId = 3
        };
    }

    [TestMethod]
    public void CreatePointsHistory_IsNotNull()
    {
        Assert.IsNotNull(_pointsHistory);
    }

    [TestMethod]
    public void CreatePointsHistory_WithEmptyConstructor_IsNotNull()
    {
        var history = new PointsHistory();
        Assert.IsNotNull(history);
    }

    [TestMethod]
    public void CreatePointsHistory_WithEmptyConstructor_OriginIsEmpty()
    {
        var history = new PointsHistory();
        Assert.AreEqual(string.Empty, history.Origin);
    }

    [TestMethod]
    public void CreatePointsHistory_WithEmptyConstructor_DescriptionIsEmpty()
    {
        var history = new PointsHistory();
        Assert.AreEqual(string.Empty, history.Description);
    }

    [TestMethod]
    public void CreatePointsHistory_WithEmptyConstructor_ActiveStrategyIsEmpty()
    {
        var history = new PointsHistory();
        Assert.AreEqual(string.Empty, history.ActiveStrategy);
    }

    [TestMethod]
    public void CreatePointsHistory_WithEmptyConstructor_TransactionDateIsSet()
    {
        var before = DateTime.Now;
        var history = new PointsHistory();
        var after = DateTime.Now;
        Assert.IsTrue(history.TransactionDate >= before && history.TransactionDate <= after);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _pointsHistory.Id = 99;
        Assert.AreEqual(99, _pointsHistory.Id);
    }

    [TestMethod]
    public void SetVisitorParkInformationId_GetCorrectly()
    {
        _pointsHistory.VisitorParkInformationId = 20;
        Assert.AreEqual(20, _pointsHistory.VisitorParkInformationId);
    }

    [TestMethod]
    public void SetTransactionDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 11, 1);
        _pointsHistory.TransactionDate = newDate;
        Assert.AreEqual(newDate, _pointsHistory.TransactionDate);
    }

    [TestMethod]
    public void SetPoints_Positive_GetCorrectly()
    {
        _pointsHistory.Points = 250;
        Assert.AreEqual(250, _pointsHistory.Points);
    }

    [TestMethod]
    public void SetPoints_Negative_GetCorrectly()
    {
        _pointsHistory.Points = -100;
        Assert.AreEqual(-100, _pointsHistory.Points);
    }

    [TestMethod]
    public void SetPoints_Zero_GetCorrectly()
    {
        _pointsHistory.Points = 0;
        Assert.AreEqual(0, _pointsHistory.Points);
    }

    [TestMethod]
    public void SetOrigin_GetCorrectly()
    {
        _pointsHistory.Origin = "Reward";
        Assert.AreEqual("Reward", _pointsHistory.Origin);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _pointsHistory.Description = "New description";
        Assert.AreEqual("New description", _pointsHistory.Description);
    }

    [TestMethod]
    public void SetActiveStrategy_GetCorrectly()
    {
        _pointsHistory.ActiveStrategy = "Combo";
        Assert.AreEqual("Combo", _pointsHistory.ActiveStrategy);
    }

    [TestMethod]
    public void SetRelatedEntityId_GetCorrectly()
    {
        _pointsHistory.RelatedEntityId = 10;
        Assert.AreEqual(10, _pointsHistory.RelatedEntityId);
    }

    [TestMethod]
    public void SetRelatedEntityId_ToNull_GetNull()
    {
        _pointsHistory.RelatedEntityId = null;
        Assert.IsNull(_pointsHistory.RelatedEntityId);
    }
}
