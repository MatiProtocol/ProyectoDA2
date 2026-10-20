using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class CreateRewardRequestTest
{
    private CreateRewardRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new CreateRewardRequest
        {
            Name = "Free Ticket",
            Description = "One free entrance",
            PointsCost = 1000,
            AvailableQuantity = 10,
            RequiredMembershipLevel = "Premium"
        };
    }

    [TestMethod]
    public void CreateRewardRequest_IsNotNull()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void ToEntity_WithoutId_CreatesEntityWithDefaultId()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(0, entity.Id);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _request.Name = "Discount Coupon";
        Assert.AreEqual("Discount Coupon", _request.Name);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _request.Description = "50% off";
        Assert.AreEqual("50% off", _request.Description);
    }

    [TestMethod]
    public void SetPointsCost_GetCorrectly()
    {
        _request.PointsCost = 500;
        Assert.AreEqual(500, _request.PointsCost);
    }

    [TestMethod]
    public void SetAvailableQuantity_GetCorrectly()
    {
        _request.AvailableQuantity = 20;
        Assert.AreEqual(20, _request.AvailableQuantity);
    }

    [TestMethod]
    public void SetRequiredMembershipLevel_GetCorrectly()
    {
        _request.RequiredMembershipLevel = "VIP";
        Assert.AreEqual("VIP", _request.RequiredMembershipLevel);
    }

    [TestMethod]
    public void SetRequiredMembershipLevelToNull_GetCorrectly()
    {
        _request.RequiredMembershipLevel = null;
        Assert.IsNull(_request.RequiredMembershipLevel);
    }

    [TestMethod]
    public void ToEntity_SetsActiveToTrue()
    {
        var entity = _request.ToEntity();
        Assert.IsTrue(entity.Active);
    }

    [TestMethod]
    public void ToEntity_WithId_CreatesEntityWithSpecifiedId()
    {
        var entity = _request.ToEntity(5);
        Assert.AreEqual(5, entity.Id);
    }

    [TestMethod]
    public void ToEntity_MapsNameCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual("Free Ticket", entity.Name);
    }

    [TestMethod]
    public void ToEntity_MapsDescriptionCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual("One free entrance", entity.Description);
    }

    [TestMethod]
    public void ToEntity_MapsPointsCostCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(1000, entity.PointsCost);
    }

    [TestMethod]
    public void ToEntity_MapsAvailableQuantityCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual(10, entity.AvailableQuantity);
    }

    [TestMethod]
    public void ToEntity_MapsRequiredMembershipLevelCorrectly()
    {
        var entity = _request.ToEntity();
        Assert.AreEqual("Premium", entity.RequiredMembershipLevel);
    }

    [TestMethod]
    public void ToEntity_MapsRequiredMembershipLevelAsNullCorrectly()
    {
        _request.RequiredMembershipLevel = null;
        var entity = _request.ToEntity();
        Assert.IsNull(entity.RequiredMembershipLevel);
    }

    [TestMethod]
    public void ToEntity_MultipleCallsCreateSeparateInstances()
    {
        var entity1 = _request.ToEntity();
        var entity2 = _request.ToEntity();
        Assert.AreNotSame(entity1, entity2);
    }
}
