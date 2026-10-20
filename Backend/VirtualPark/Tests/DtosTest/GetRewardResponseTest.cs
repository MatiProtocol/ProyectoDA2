using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetRewardResponseTest
{
    private Reward _reward = null!;
    private GetRewardResponse _response = null!;

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
            RequiredMembershipLevel = "Premium",
            Active = true
        };
        _response = new GetRewardResponse(_reward);
    }

    [TestMethod]
    public void GetRewardResponse_IsNotNull()
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
    public void Constructor_MapsRequiredMembershipLevelCorrectly()
    {
        Assert.AreEqual("Premium", _response.RequiredMembershipLevel);
    }

    [TestMethod]
    public void Constructor_MapsActiveCorrectly()
    {
        Assert.IsTrue(_response.Active);
    }

    [TestMethod]
    public void Constructor_MapsIsAvailableCorrectly()
    {
        Assert.IsTrue(_response.IsAvailable);
    }

    [TestMethod]
    public void Constructor_MapsRequiredMembershipLevelAsNullCorrectly()
    {
        _reward.RequiredMembershipLevel = null;
        var response = new GetRewardResponse(_reward);
        Assert.IsNull(response.RequiredMembershipLevel);
    }

    [TestMethod]
    public void Constructor_MapsIsAvailableFalseWhenInactive()
    {
        _reward.Active = false;
        var response = new GetRewardResponse(_reward);
        Assert.IsFalse(response.IsAvailable);
    }

    [TestMethod]
    public void Constructor_MapsIsAvailableFalseWhenOutOfStock()
    {
        _reward.AvailableQuantity = 0;
        var response = new GetRewardResponse(_reward);
        Assert.IsFalse(response.IsAvailable);
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
    public void SetRequiredMembershipLevel_GetCorrectly()
    {
        _response.RequiredMembershipLevel = "VIP";
        Assert.AreEqual("VIP", _response.RequiredMembershipLevel);
    }

    [TestMethod]
    public void SetRequiredMembershipLevelToNull_GetCorrectly()
    {
        _response.RequiredMembershipLevel = null;
        Assert.IsNull(_response.RequiredMembershipLevel);
    }

    [TestMethod]
    public void SetActive_GetCorrectly()
    {
        _response.Active = false;
        Assert.IsFalse(_response.Active);
    }

    [TestMethod]
    public void SetIsAvailable_GetCorrectly()
    {
        _response.IsAvailable = false;
        Assert.IsFalse(_response.IsAvailable);
    }
}
