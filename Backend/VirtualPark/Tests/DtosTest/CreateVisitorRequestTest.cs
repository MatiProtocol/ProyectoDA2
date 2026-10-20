using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class CreateVisitorRequestTest
{
    private CreateVisitorRequest _request = null!;
    private DateTime _birthdate;

    [TestInitialize]
    public void Initialize()
    {
        _birthdate = new DateTime(1990, 5, 15);
        _request = new CreateVisitorRequest
        {
            Name = "John Doe",
            Email = "john@example.com",
            Birthdate = _birthdate,
            NewPassword = "SecPass123",
            OldPassword = "SecurePass123"
        };
    }

    [TestMethod]
    public void CreateVisitorRequest_WithValidData_CreatesInstance()
    {
        Assert.IsNotNull(_request);
    }

    [TestMethod]
    public void CreateVisitorRequest_SetsName()
    {
        Assert.AreEqual("John Doe", _request.Name);
    }

    [TestMethod]
    public void CreateVisitorRequest_SetNewPassword_GetNewPassword()
    {
        _request.NewPassword = "Jesus";
        Assert.AreEqual("Jesus", _request.NewPassword);
    }

    [TestMethod]
    public void CreateVisitorRequest_SetOldPassword_GetOldPassword()
    {
        _request.OldPassword = "Meu deus";
        Assert.AreEqual("Meu deus", _request.OldPassword);
    }

    [TestMethod]
    public void CreateVisitorRequest_SetsEmail()
    {
        Assert.AreEqual("john@example.com", _request.Email);
    }

    [TestMethod]
    public void CreateVisitorRequest_SetsBirthdate()
    {
        Assert.AreEqual(_birthdate, _request.Birthdate);
    }

    [TestMethod]
    public void CreateVisitorRequest_SetsPassword()
    {
        Assert.AreEqual("SecurePass123", _request.OldPassword);
    }

    [TestMethod]
    public void ToEntity_WithoutId_CreatesVisitorWithDefaultId()
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
    public void ToEntity_WithoutId_SetsBirthDate()
    {
        var entity = _request.ToEntity();

        Assert.AreEqual(_birthdate, entity.BirthDate);
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
    public void ToEntity_WithId_SetsBirthDate()
    {
        var entity = _request.ToEntity(10);

        Assert.AreEqual(_birthdate, entity.BirthDate);
    }

    [TestMethod]
    public void ToEntity_MultipleCallsSameRequest_CreatesSeparateInstances()
    {
        var entity1 = _request.ToEntity();
        var entity2 = _request.ToEntity();

        Assert.AreNotSame(entity1, entity2);
    }
}
