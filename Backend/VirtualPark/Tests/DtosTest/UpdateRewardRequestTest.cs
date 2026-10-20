using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class UpdateRewardRequestTest
{
    private UpdateRewardRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new UpdateRewardRequest
        {
            Name = "Free Ticket",
            Description = "One free entrance",
            PointsCost = 1000,
            AvailableQuantity = 10,
            RequiredMembershipLevel = "Premium",
            Active = true
        };
    }

    [TestMethod]
    public void UpdateRewardRequest_IsNotNull()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void SetName_GetCorrectly()
    {
        _request.Name = "Updated Name";
        Assert.AreEqual("Updated Name", _request.Name);
    }

    [TestMethod]
    public void SetDescription_GetCorrectly()
    {
        _request.Description = "Updated Description";
        Assert.AreEqual("Updated Description", _request.Description);
    }

    [TestMethod]
    public void SetPointsCost_GetCorrectly()
    {
        _request.PointsCost = 1500;
        Assert.AreEqual(1500, _request.PointsCost);
    }

    [TestMethod]
    public void SetAvailableQuantity_GetCorrectly()
    {
        _request.AvailableQuantity = 5;
        Assert.AreEqual(5, _request.AvailableQuantity);
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
    public void SetActive_GetCorrectly()
    {
        _request.Active = false;
        Assert.IsFalse(_request.Active);
    }

    [TestMethod]
    public void ToEntity_MapsIdCorrectly()
    {
        var entity = _request.ToEntity(7);
        Assert.AreEqual(7, entity.Id);
    }

    [TestMethod]
    public void ToEntity_MapsNameCorrectly()
    {
        var entity = _request.ToEntity(1);
        Assert.AreEqual("Free Ticket", entity.Name);
    }

    [TestMethod]
    public void ToEntity_MapsDescriptionCorrectly()
    {
        var entity = _request.ToEntity(1);
        Assert.AreEqual("One free entrance", entity.Description);
    }

    [TestMethod]
    public void ToEntity_MapsPointsCostCorrectly()
    {
        var entity = _request.ToEntity(1);
        Assert.AreEqual(1000, entity.PointsCost);
    }

    [TestMethod]
    public void ToEntity_MapsAvailableQuantityCorrectly()
    {
        var entity = _request.ToEntity(1);
        Assert.AreEqual(10, entity.AvailableQuantity);
    }

    [TestMethod]
    public void ToEntity_MapsRequiredMembershipLevelCorrectly()
    {
        var entity = _request.ToEntity(1);
        Assert.AreEqual("Premium", entity.RequiredMembershipLevel);
    }

    [TestMethod]
    public void ToEntity_MapsRequiredMembershipLevelAsNullCorrectly()
    {
        _request.RequiredMembershipLevel = null;
        var entity = _request.ToEntity(1);
        Assert.IsNull(entity.RequiredMembershipLevel);
    }

    [TestMethod]
    public void ToEntity_MapsActiveCorrectly()
    {
        var entity = _request.ToEntity(1);
        Assert.IsTrue(entity.Active);
    }

    [TestMethod]
    public void ToEntity_MapsActiveFalseCorrectly()
    {
        _request.Active = false;
        var entity = _request.ToEntity(1);
        Assert.IsFalse(entity.Active);
    }

    [TestMethod]
    public void ToEntity_MultipleCallsCreateSeparateInstances()
    {
        var entity1 = _request.ToEntity(1);
        var entity2 = _request.ToEntity(1);
        Assert.AreNotSame(entity1, entity2);
    }
}
