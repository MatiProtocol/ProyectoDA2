using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetAvailableRewardsResponseTest
{
    private List<Reward> _rewards = null!;
    private GetAvailableRewardsResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _rewards =
        [
            new Reward
            {
                Id = 1,
                Name = "Free Ticket",
                Description = "One free entrance",
                PointsCost = 1000,
                AvailableQuantity = 10,
                Active = true
            },
            new Reward
            {
                Id = 2,
                Name = "Discount Coupon",
                Description = "50% off",
                PointsCost = 500,
                AvailableQuantity = 5,
                Active = true
            }

        ];
        _response = new GetAvailableRewardsResponse(_rewards, 2000, "Premium");
    }

    [TestMethod]
    public void GetAvailableRewardsResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsVisitorPointsCorrectly()
    {
        Assert.AreEqual(2000, _response.VisitorPoints);
    }

    [TestMethod]
    public void Constructor_MapsVisitorMembershipCorrectly()
    {
        Assert.AreEqual("Premium", _response.VisitorMembership);
    }

    [TestMethod]
    public void Constructor_MapsRewardsListCorrectly()
    {
        Assert.AreEqual(2, _response.Rewards.Count);
    }

    [TestMethod]
    public void Constructor_CreatesRewardItemResponses()
    {
        Assert.IsInstanceOfType(_response.Rewards[0], typeof(RewardItemResponse));
    }

    [TestMethod]
    public void Constructor_WithEmptyRewardsList_CreatesEmptyList()
    {
        var response = new GetAvailableRewardsResponse([], 1000, "Basic");
        Assert.AreEqual(0, response.Rewards.Count);
    }

    [TestMethod]
    public void SetRewards_GetCorrectly()
    {
        var newRewards = new List<RewardItemResponse>
        {
            new RewardItemResponse(new Reward { Id = 3, Name = "New Reward" })
        };
        _response.Rewards = newRewards;
        Assert.AreEqual(1, _response.Rewards.Count);
    }

    [TestMethod]
    public void SetVisitorPoints_GetCorrectly()
    {
        _response.VisitorPoints = 1500;
        Assert.AreEqual(1500, _response.VisitorPoints);
    }

    [TestMethod]
    public void SetVisitorMembership_GetCorrectly()
    {
        _response.VisitorMembership = "VIP";
        Assert.AreEqual("VIP", _response.VisitorMembership);
    }

    [TestMethod]
    public void SetRewardsToNull_GetCorrectly()
    {
        _response.Rewards = null!;
        Assert.IsNull(_response.Rewards);
    }

    [TestMethod]
    public void SetVisitorPointsToZero_GetCorrectly()
    {
        _response.VisitorPoints = 0;
        Assert.AreEqual(0, _response.VisitorPoints);
    }

    [TestMethod]
    public void SetVisitorMembershipToEmpty_GetCorrectly()
    {
        _response.VisitorMembership = " ";
        Assert.AreEqual(" ", _response.VisitorMembership);
    }
}

[TestClass]
public class RewardItemResponseTest
{
    private Reward _reward = null!;
    private RewardItemResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _reward = new Reward
        {
            Id = 1,
            Name = "Free Ticket",
            Description = "One free entrance",
            PointsCost = 1000,
            AvailableQuantity = 10,
            Active = true
        };
        _response = new RewardItemResponse(_reward);
    }

    [TestMethod]
    public void RewardItemResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void Constructor_MapsIdCorrectly()
    {
        Assert.AreEqual(1, _response.Id);
    }

    [TestMethod]
    public void Constructor_MapsNameCorrectly()
    {
        Assert.AreEqual("Free Ticket", _response.Name);
    }

    [TestMethod]
    public void Constructor_MapsDescriptionCorrectly()
    {
        Assert.AreEqual("One free entrance", _response.Description);
    }

    [TestMethod]
    public void Constructor_MapsPointsCostCorrectly()
    {
        Assert.AreEqual(1000, _response.PointsCost);
    }

    [TestMethod]
    public void Constructor_MapsAvailableQuantityCorrectly()
    {
        Assert.AreEqual(10, _response.AvailableQuantity);
    }

    [TestMethod]
    public void Constructor_MapsCanRedeemCorrectly()
    {
        Assert.IsTrue(_response.CanRedeem);
    }

    [TestMethod]
    public void Constructor_MapsCanRedeemFalseWhenInactive()
    {
        _reward.Active = false;
        var response = new RewardItemResponse(_reward);
        Assert.IsFalse(response.CanRedeem);
    }

    [TestMethod]
    public void Constructor_MapsCanRedeemFalseWhenOutOfStock()
    {
        _reward.AvailableQuantity = 0;
        var response = new RewardItemResponse(_reward);
        Assert.IsFalse(response.CanRedeem);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _response.Id = 5;
        Assert.AreEqual(5, _response.Id);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _response.Name = "Updated Name";
        Assert.AreEqual("Updated Name", _response.Name);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _response.Description = "Updated Description";
        Assert.AreEqual("Updated Description", _response.Description);
    }

    [TestMethod]
    public void SetPointsCost_GetCorrectly()
    {
        _response.PointsCost = 1500;
        Assert.AreEqual(1500, _response.PointsCost);
    }

    [TestMethod]
    public void SetAvailableQuantity_GetCorrectly()
    {
        _response.AvailableQuantity = 5;
        Assert.AreEqual(5, _response.AvailableQuantity);
    }

    [TestMethod]
    public void SetCanRedeem_GetCorrectly()
    {
        _response.CanRedeem = false;
        Assert.IsFalse(_response.CanRedeem);
    }

    [TestMethod]
    public void SetPointsCostToZero_GetCorrectly()
    {
        _response.PointsCost = 0;
        Assert.AreEqual(0, _response.PointsCost);
    }

    [TestMethod]
    public void SetAvailableQuantityToZero_GetCorrectly()
    {
        _response.AvailableQuantity = 0;
        Assert.AreEqual(0, _response.AvailableQuantity);
    }

    [TestMethod]
    public void SetAvailableQuantityToNegative_GetCorrectly()
    {
        _response.AvailableQuantity = -1;
        Assert.AreEqual(-1, _response.AvailableQuantity);
    }
}
