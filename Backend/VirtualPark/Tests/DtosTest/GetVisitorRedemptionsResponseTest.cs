using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetVisitorRedemptionsResponseTest
{
    private List<RewardRedemption> _redemptions = null!;
    private GetVisitorRedemptionsResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _redemptions =
        [
            new RewardRedemption
            {
                Id = 1,
                RewardId = 1,
                RedemptionDate = new DateTime(2025, 10, 15, 14, 30, 0),
                PointsSpent = 1000,
                Status = RedemptionStatus.Completed
            },
            new RewardRedemption
            {
                Id = 2,
                RewardId = 2,
                RedemptionDate = new DateTime(2025, 10, 16, 10, 0, 0),
                PointsSpent = 500,
                Status = RedemptionStatus.Completed
            }

        ];
        _response = new GetVisitorRedemptionsResponse(_redemptions);
    }

    [TestMethod]
    public void GetVisitorRedemptionsResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsRedemptionsListCorrectly()
    {
        Assert.AreEqual(2, _response.Redemptions.Count);
    }

    [TestMethod]
    public void Constructor_MapsTotalPointsRedeemedCorrectly()
    {
        Assert.AreEqual(1500, _response.TotalPointsRedeemed);
    }

    [TestMethod]
    public void Constructor_CreatesRedemptionHistoryItems()
    {
        Assert.IsInstanceOfType(_response.Redemptions[0], typeof(RedemptionHistoryItem));
    }

    [TestMethod]
    public void Constructor_WithEmptyRedemptionsList_CreatesEmptyList()
    {
        var response = new GetVisitorRedemptionsResponse([]);
        Assert.AreEqual(0, response.Redemptions.Count);
    }

    [TestMethod]
    public void Constructor_WithEmptyRedemptionsList_SetsTotalPointsToZero()
    {
        var response = new GetVisitorRedemptionsResponse([]);
        Assert.AreEqual(0, response.TotalPointsRedeemed);
    }

    [TestMethod]
    public void Constructor_WithSingleRedemption_CalculatesTotalPointsCorrectly()
    {
        var redemptions = new List<RewardRedemption>
        {
            new RewardRedemption { PointsSpent = 750, Status = RedemptionStatus.Completed }
        };
        var response = new GetVisitorRedemptionsResponse(redemptions);
        Assert.AreEqual(750, response.TotalPointsRedeemed);
    }

    [TestMethod]
    public void SetRedemptions_GetCorrectly()
    {
        var newRedemptions = new List<RedemptionHistoryItem>
        {
            new RedemptionHistoryItem(new RewardRedemption { Id = 3 })
        };
        _response.Redemptions = newRedemptions;
        Assert.AreEqual(1, _response.Redemptions.Count);
    }

    [TestMethod]
    public void SetTotalPointsRedeemed_GetCorrectly()
    {
        _response.TotalPointsRedeemed = 2000;
        Assert.AreEqual(2000, _response.TotalPointsRedeemed);
    }

    [TestMethod]
    public void SetRedemptionsToNull_GetCorrectly()
    {
        _response.Redemptions = null!;
        Assert.IsNull(_response.Redemptions);
    }

    [TestMethod]
    public void SetTotalPointsRedeemedToZero_GetCorrectly()
    {
        _response.TotalPointsRedeemed = 0;
        Assert.AreEqual(0, _response.TotalPointsRedeemed);
    }

    [TestMethod]
    public void SetTotalPointsRedeemedToNegative_GetCorrectly()
    {
        _response.TotalPointsRedeemed = -500;
        Assert.AreEqual(-500, _response.TotalPointsRedeemed);
    }
}

[TestClass]
public class RedemptionHistoryItemTest
{
    private RewardRedemption _redemption = null!;
    private RedemptionHistoryItem _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _redemption = new RewardRedemption
        {
            Id = 1,
            RewardId = 5,
            RedemptionDate = new DateTime(2025, 10, 15, 14, 30, 0),
            PointsSpent = 1000,
            Status = RedemptionStatus.Completed
        };
        _response = new RedemptionHistoryItem(_redemption);
    }

    [TestMethod]
    public void RedemptionHistoryItem_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsIdCorrectly()
    {
        Assert.AreEqual(1, _response.Id);
    }

    [TestMethod]
    public void Constructor_MapsRewardIdCorrectly()
    {
        Assert.AreEqual(5, _response.RewardId);
    }

    [TestMethod]
    public void Constructor_MapsPointsSpentCorrectly()
    {
        Assert.AreEqual(1000, _response.PointsSpent);
    }

    [TestMethod]
    public void Constructor_MapsRedemptionDateCorrectly()
    {
        Assert.AreEqual(new DateTime(2025, 10, 15, 14, 30, 0), _response.RedemptionDate);
    }

    [TestMethod]
    public void Constructor_MapsStatusCorrectly()
    {
        Assert.AreEqual("Completed", _response.Status);
    }

    [TestMethod]
    public void Constructor_MapsPendingStatusCorrectly()
    {
        _redemption.Status = RedemptionStatus.Pending;
        var response = new RedemptionHistoryItem(_redemption);
        Assert.AreEqual("Pending", response.Status);
    }

    [TestMethod]
    public void Constructor_MapsCancelledStatusCorrectly()
    {
        _redemption.Status = RedemptionStatus.Cancelled;
        var response = new RedemptionHistoryItem(_redemption);
        Assert.AreEqual("Cancelled", response.Status);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _response.Id = 10;
        Assert.AreEqual(10, _response.Id);
    }

    [TestMethod]
    public void SetRewardId_GetCorrectly()
    {
        _response.RewardId = 15;
        Assert.AreEqual(15, _response.RewardId);
    }

    [TestMethod]
    public void SetRedemptionDate_GetCorrectly()
    {
        var newDate = new DateTime(2025, 11, 1, 10, 0, 0);
        _response.RedemptionDate = newDate;
        Assert.AreEqual(newDate, _response.RedemptionDate);
    }

    [TestMethod]
    public void SetPointsSpent_GetCorrectly()
    {
        _response.PointsSpent = 500;
        Assert.AreEqual(500, _response.PointsSpent);
    }

    [TestMethod]
    public void SetStatus_GetCorrectly()
    {
        _response.Status = "Pending";
        Assert.AreEqual("Pending", _response.Status);
    }

    [TestMethod]
    public void SetIdToZero_GetCorrectly()
    {
        _response.Id = 0;
        Assert.AreEqual(0, _response.Id);
    }

    [TestMethod]
    public void SetRewardIdToZero_GetCorrectly()
    {
        _response.RewardId = 0;
        Assert.AreEqual(0, _response.RewardId);
    }

    [TestMethod]
    public void SetPointsSpentToZero_GetCorrectly()
    {
        _response.PointsSpent = 0;
        Assert.AreEqual(0, _response.PointsSpent);
    }

    [TestMethod]
    public void SetStatusToEmpty_GetCorrectly()
    {
        _response.Status = " ";
        Assert.AreEqual(" ", _response.Status);
    }

    [TestMethod]
    public void SetPointsSpentToNegative_GetCorrectly()
    {
        _response.PointsSpent = -100;
        Assert.AreEqual(-100, _response.PointsSpent);
    }
}
