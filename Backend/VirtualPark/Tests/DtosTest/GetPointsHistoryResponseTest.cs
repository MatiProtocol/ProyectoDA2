using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetPointsHistoryResponseTest
{
    private List<PointsHistory> _history = null!;
    private GetPointsHistoryResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _history =
        [
            new PointsHistory
            {
                Id = 1,
                TransactionDate = new DateTime(2025, 10, 15, 10, 0, 0),
                Points = 100,
                Origin = "Attraction",
                Description = "Completed roller coaster",
                ActiveStrategy = "AttractionBased"
            },
            new PointsHistory
            {
                Id = 2,
                TransactionDate = new DateTime(2025, 10, 15, 11, 0, 0),
                Points = -50,
                Origin = "Reward",
                Description = "Redeemed discount",
                ActiveStrategy = "AttractionBased"
            },
            new PointsHistory
            {
                Id = 3,
                TransactionDate = new DateTime(2025, 10, 15, 12, 0, 0),
                Points = 200,
                Origin = "SpecialEvent",
                Description = "Completed event",
                ActiveStrategy = "EventBased"
            }

        ];
        _response = new GetPointsHistoryResponse(_history, 250);
    }

    [TestMethod]
    public void GetPointsHistoryResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsHistoryListCorrectly()
    {
        Assert.AreEqual(3, _response.History.Count);
    }

    [TestMethod]
    public void Constructor_MapsTotalPointsEarnedCorrectly()
    {
        Assert.AreEqual(300, _response.TotalPointsEarned);
    }

    [TestMethod]
    public void Constructor_MapsTotalPointsSpentCorrectly()
    {
        Assert.AreEqual(50, _response.TotalPointsSpent);
    }

    [TestMethod]
    public void Constructor_MapsCurrentBalanceCorrectly()
    {
        Assert.AreEqual(250, _response.CurrentBalance);
    }

    [TestMethod]
    public void Constructor_CreatesPointsHistoryItems()
    {
        Assert.IsInstanceOfType(_response.History[0], typeof(PointsHistoryItem));
    }

    [TestMethod]
    public void Constructor_WithEmptyHistoryList_CreatesEmptyList()
    {
        var response = new GetPointsHistoryResponse([], 0);
        Assert.AreEqual(0, response.History.Count);
    }

    [TestMethod]
    public void Constructor_WithEmptyHistoryList_SetsAllTotalsToZero()
    {
        var response = new GetPointsHistoryResponse([], 0);
        Assert.AreEqual(0, response.TotalPointsEarned);
        Assert.AreEqual(0, response.TotalPointsSpent);
        Assert.AreEqual(0, response.CurrentBalance);
    }

    [TestMethod]
    public void Constructor_WithOnlyPositivePoints_CalculatesTotalsCorrectly()
    {
        var history = new List<PointsHistory>
        {
            new PointsHistory { Points = 100 },
            new PointsHistory { Points = 200 }
        };
        var response = new GetPointsHistoryResponse(history, 300);
        Assert.AreEqual(300, response.TotalPointsEarned);
        Assert.AreEqual(0, response.TotalPointsSpent);
    }

    [TestMethod]
    public void Constructor_WithOnlyNegativePoints_CalculatesTotalsCorrectly()
    {
        var history = new List<PointsHistory>
        {
            new PointsHistory { Points = -50 },
            new PointsHistory { Points = -100 }
        };
        var response = new GetPointsHistoryResponse(history, -150);
        Assert.AreEqual(0, response.TotalPointsEarned);
        Assert.AreEqual(150, response.TotalPointsSpent);
    }

    [TestMethod]
    public void SetHistory_GetCorrectly()
    {
        var newHistory = new List<PointsHistoryItem>
        {
            new PointsHistoryItem(new PointsHistory { Id = 4 })
        };
        _response.History = newHistory;
        Assert.AreEqual(1, _response.History.Count);
    }

    [TestMethod]
    public void SetTotalPointsEarned_GetCorrectly()
    {
        _response.TotalPointsEarned = 500;
        Assert.AreEqual(500, _response.TotalPointsEarned);
    }

    [TestMethod]
    public void SetTotalPointsSpent_GetCorrectly()
    {
        _response.TotalPointsSpent = 100;
        Assert.AreEqual(100, _response.TotalPointsSpent);
    }

    [TestMethod]
    public void SetCurrentBalance_GetCorrectly()
    {
        _response.CurrentBalance = 1000;
        Assert.AreEqual(1000, _response.CurrentBalance);
    }

    [TestMethod]
    public void SetHistoryToNull_GetCorrectly()
    {
        _response.History = null!;
        Assert.IsNull(_response.History);
    }

    [TestMethod]
    public void SetTotalPointsEarnedToZero_GetCorrectly()
    {
        _response.TotalPointsEarned = 0;
        Assert.AreEqual(0, _response.TotalPointsEarned);
    }

    [TestMethod]
    public void SetTotalPointsSpentToZero_GetCorrectly()
    {
        _response.TotalPointsSpent = 0;
        Assert.AreEqual(0, _response.TotalPointsSpent);
    }

    [TestMethod]
    public void SetCurrentBalanceToZero_GetCorrectly()
    {
        _response.CurrentBalance = 0;
        Assert.AreEqual(0, _response.CurrentBalance);
    }

    [TestMethod]
    public void SetCurrentBalanceToNegative_GetCorrectly()
    {
        _response.CurrentBalance = -100;
        Assert.AreEqual(-100, _response.CurrentBalance);
    }
}

[TestClass]
public class PointsHistoryItemTest
{
    private PointsHistory _history = null!;
    private PointsHistoryItem _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _history = new PointsHistory
        {
            Id = 1,
            TransactionDate = new DateTime(2025, 10, 15, 14, 30, 0),
            Points = 100,
            Origin = "Attraction",
            Description = "Completed roller coaster",
            ActiveStrategy = "AttractionBased"
        };
        _response = new PointsHistoryItem(_history);
    }

    [TestMethod]
    public void PointsHistoryItem_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsIdCorrectly()
    {
        Assert.AreEqual(1, _response.Id);
    }

    [TestMethod]
    public void Constructor_MapsTransactionDateCorrectly()
    {
        Assert.AreEqual(new DateTime(2025, 10, 15, 14, 30, 0), _response.TransactionDate);
    }

    [TestMethod]
    public void Constructor_MapsPointsCorrectly()
    {
        Assert.AreEqual(100, _response.Points);
    }

    [TestMethod]
    public void Constructor_MapsOriginCorrectly()
    {
        Assert.AreEqual("Attraction", _response.Origin);
    }

    [TestMethod]
    public void Constructor_MapsDescriptionCorrectly()
    {
        Assert.AreEqual("Completed roller coaster", _response.Description);
    }

    [TestMethod]
    public void Constructor_MapsActiveStrategyCorrectly()
    {
        Assert.AreEqual("AttractionBased", _response.ActiveStrategy);
    }

    [TestMethod]
    public void Constructor_MapsNegativePointsCorrectly()
    {
        _history.Points = -50;
        var response = new PointsHistoryItem(_history);
        Assert.AreEqual(-50, response.Points);
    }

    [TestMethod]
    public void Constructor_MapsZeroPointsCorrectly()
    {
        _history.Points = 0;
        var response = new PointsHistoryItem(_history);
        Assert.AreEqual(0, response.Points);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _response.Id = 5;
        Assert.AreEqual(5, _response.Id);
    }

    [TestMethod]
    public void SetTransactionDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 11, 1, 10, 0, 0);
        _response.TransactionDate = newDate;
        Assert.AreEqual(newDate, _response.TransactionDate);
    }

    [TestMethod]
    public void SetPoints_GetCorrectly()
    {
        _response.Points = 200;
        Assert.AreEqual(200, _response.Points);
    }

    [TestMethod]
    public void SetOrigin_GetCorrectly()
    {
        _response.Origin = "Reward";
        Assert.AreEqual("Reward", _response.Origin);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _response.Description = "Updated description";
        Assert.AreEqual("Updated description", _response.Description);
    }

    [TestMethod]
    public void SetActiveStrategy_GetCorrectly()
    {
        _response.ActiveStrategy = "EventBased";
        Assert.AreEqual("EventBased", _response.ActiveStrategy);
    }

    [TestMethod]
    public void SetIdToZero_GetCorrectly()
    {
        _response.Id = 0;
        Assert.AreEqual(0, _response.Id);
    }

    [TestMethod]
    public void SetPointsToZero_GetCorrectly()
    {
        _response.Points = 0;
        Assert.AreEqual(0, _response.Points);
    }

    [TestMethod]
    public void SetPointsToNegative_GetCorrectly()
    {
        _response.Points = -100;
        Assert.AreEqual(-100, _response.Points);
    }

    [TestMethod]
    public void SetOriginToEmpty_GetCorrectly()
    {
        _response.Origin = " ";
        Assert.AreEqual(" ", _response.Origin);
    }

    [TestMethod]
    public void SetDescriptionToEmpty_GetCorrectly()
    {
        _response.Description = " ";
        Assert.AreEqual(" ", _response.Description);
    }

    [TestMethod]
    public void SetActiveStrategyToEmpty_GetCorrectly()
    {
        _response.ActiveStrategy = " ";
        Assert.AreEqual(" ", _response.ActiveStrategy);
    }
}
