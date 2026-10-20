using Domain;

namespace Tests.DomainTest;

[TestClass]
public class RewardTest
{
    private Reward _reward = null!;

    [TestInitialize]
    public void Initialize()
    {
        _reward = new Reward
        {
            Id = 1,
            Name = "Free Ticket",
            Description = "One free entrance ticket",
            PointsCost = 1000,
            AvailableQuantity = 50,
            RequiredMembershipLevel = "Premium",
            Active = true
        };
    }

    [TestMethod]
    public void CreateReward_IsNotNull()
    {
        Assert.IsNotNull(_reward);
    }

    [TestMethod]
    public void CreateReward_WithEmptyConstructor_IsNotNull()
    {
        var reward = new Reward();
        Assert.IsNotNull(reward);
    }

    [TestMethod]
    public void CreateReward_WithEmptyConstructor_NameIsEmpty()
    {
        var reward = new Reward();
        Assert.AreEqual(string.Empty, reward.Name);
    }

    [TestMethod]
    public void CreateReward_WithEmptyConstructor_DescriptionIsEmpty()
    {
        var reward = new Reward();
        Assert.AreEqual(string.Empty, reward.Description);
    }

    [TestMethod]
    public void CreateReward_WithEmptyConstructor_ActiveIsTrue()
    {
        var reward = new Reward();
        Assert.IsTrue(reward.Active);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        _reward.Id = 5;
        Assert.AreEqual(5, _reward.Id);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _reward.Name = "VIP Pass";
        Assert.AreEqual("VIP Pass", _reward.Name);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _reward.Description = "New description";
        Assert.AreEqual("New description", _reward.Description);
    }

    [TestMethod]
    public void SetPointsCost_GetCorrectly()
    {
        _reward.PointsCost = 500;
        Assert.AreEqual(500, _reward.PointsCost);
    }

    [TestMethod]
    public void SetAvailableQuantity_GetCorrectly()
    {
        _reward.AvailableQuantity = 100;
        Assert.AreEqual(100, _reward.AvailableQuantity);
    }

    [TestMethod]
    public void SetRequiredMembershipLevel_GetCorrectly()
    {
        _reward.RequiredMembershipLevel = "VIP";
        Assert.AreEqual("VIP", _reward.RequiredMembershipLevel);
    }

    [TestMethod]
    public void SetRequiredMembershipLevel_ToNull_GetNull()
    {
        _reward.RequiredMembershipLevel = null;
        Assert.IsNull(_reward.RequiredMembershipLevel);
    }

    [TestMethod]
    public void SetActive_GetCorrectly()
    {
        _reward.Active = false;
        Assert.IsFalse(_reward.Active);
    }

    [TestMethod]
    public void IsAvailable_WhenActiveAndHasStock_ReturnsTrue()
    {
        _reward.Active = true;
        _reward.AvailableQuantity = 10;
        Assert.IsTrue(_reward.IsAvailable());
    }

    [TestMethod]
    public void IsAvailable_WhenInactive_ReturnsFalse()
    {
        _reward.Active = false;
        _reward.AvailableQuantity = 10;
        Assert.IsFalse(_reward.IsAvailable());
    }

    [TestMethod]
    public void IsAvailable_WhenNoStock_ReturnsFalse()
    {
        _reward.Active = true;
        _reward.AvailableQuantity = 0;
        Assert.IsFalse(_reward.IsAvailable());
    }

    [TestMethod]
    public void CanBeRedeemedByMembership_WhenNoRequirement_ReturnsTrue()
    {
        _reward.RequiredMembershipLevel = null;
        Assert.IsTrue(_reward.CanBeRedeemedByMembership("Standard"));
    }

    [TestMethod]
    public void CanBeRedeemedByMembership_WhenEmptyRequirement_ReturnsTrue()
    {
        _reward.RequiredMembershipLevel = string.Empty;
        Assert.IsTrue(_reward.CanBeRedeemedByMembership("Standard"));
    }

    [TestMethod]
    public void CanBeRedeemedByMembership_WhenMatchesCaseInsensitive_ReturnsTrue()
    {
        _reward.RequiredMembershipLevel = "Premium";
        Assert.IsTrue(_reward.CanBeRedeemedByMembership("premium"));
    }

    [TestMethod]
    public void CanBeRedeemedByMembership_WhenDoesNotMatch_ReturnsFalse()
    {
        _reward.RequiredMembershipLevel = "VIP";
        Assert.IsFalse(_reward.CanBeRedeemedByMembership("Standard"));
    }

    [TestMethod]
    public void DecrementStock_WhenHasStock_ReturnsTrue()
    {
        _reward.AvailableQuantity = 5;
        Assert.IsTrue(_reward.DecrementStock());
    }

    [TestMethod]
    public void DecrementStock_WhenHasStock_DecrementsQuantity()
    {
        _reward.AvailableQuantity = 5;
        _reward.DecrementStock();
        Assert.AreEqual(4, _reward.AvailableQuantity);
    }

    [TestMethod]
    public void DecrementStock_WhenNoStock_ReturnsFalse()
    {
        _reward.AvailableQuantity = 0;
        Assert.IsFalse(_reward.DecrementStock());
    }

    [TestMethod]
    public void DecrementStock_WhenNoStock_DoesNotChangeQuantity()
    {
        _reward.AvailableQuantity = 0;
        _reward.DecrementStock();
        Assert.AreEqual(0, _reward.AvailableQuantity);
    }
}
