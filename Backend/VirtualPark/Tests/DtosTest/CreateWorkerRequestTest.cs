using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class CreateWorkerRequestTest
{
    private CreateWorkerRequest _request = null!;

    [TestInitialize]
    public void Initialize()
    {
        _request = new CreateWorkerRequest
        {
            Name = "John Doe",
            Email = "john@example.com",
            OldPassword = "SecPass123",

            NewPassword = "SecurePass123",
            Roles = "ParkAdmin,"
        };
    }

    [TestMethod]
    public void CreateWorkerRequest_WithValidData_CreatesInstance()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void CreateWorkerRequest_SetsName()
    {
        Assert.AreEqual("John Doe", _request.Name);
    }

    [TestMethod]
    public void CreateWorkerRequest_SetPassword_GetPassword()
    {
        Assert.AreEqual("SecPass123", _request.OldPassword);
    }

    [TestMethod]
    public void CreateWorkerRequest_SetNewPassword_GetNewPassword()
    {
        Assert.AreEqual("SecurePass123", _request.NewPassword);
    }

    [TestMethod]
    public void CreateWorkerRequest_SetOldPassword_GetOldPassword()
    {
        Assert.AreEqual("SecurePass123", _request.NewPassword);
    }

    [TestMethod]
    public void CreateWorkerRequest_SetsEmail()
    {
        Assert.AreEqual("john@example.com", _request.Email);
    }

    [TestMethod]
    public void ToEntity_WithoutId_CreatesWorkerWithDefaultId()
    {
        var entity = _request.ToEntity();

        Assert.AreEqual(0, entity.Id);
    }

    [TestMethod]
    public void ToEntity_WithoutId_SetsName()
    {
        var entity = _request.ToEntity();

        Assert.AreEqual("John Doe", entity.Name);
    }

    [TestMethod]
    public void ToEntity_WithoutId_SetsEmail()
    {
        var entity = _request.ToEntity();

        Assert.AreEqual("john@example.com", entity.Email);
    }

    [TestMethod]
    public void ToEntity_WithId_SetsCorrectId()
    {
        var entity = _request.ToEntity(10);

        Assert.AreEqual(10, entity.Id);
    }

    [TestMethod]
    public void ToEntity_WithId_SetsName()
    {
        var entity = _request.ToEntity(10);

        Assert.AreEqual("John Doe", entity.Name);
    }

    [TestMethod]
    public void ToEntity_WithId_SetsEmail()
    {
        var entity = _request.ToEntity(10);

        Assert.AreEqual("john@example.com", entity.Email);
    }

    [TestMethod]
    public void ToEntity_MultipleCallsSameRequest_CreatesSeparateInstances()
    {
        var entity1 = _request.ToEntity();
        var entity2 = _request.ToEntity();

        Assert.AreNotSame(entity1, entity2);
    }
}
