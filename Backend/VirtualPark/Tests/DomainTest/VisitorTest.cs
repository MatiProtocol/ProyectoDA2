using Domain;

namespace Tests.DomainTest;

[TestClass]
public class VisitorTest
{
    private Visitor.VisitorConstructHelper _helper;

    [TestInitialize]
    public void Initialize()
    {
        _helper = new Visitor.VisitorConstructHelper
        {
            Name = "Mati",
            Birthdate = DateTime.Parse("10/07/2004"),
            Email = "MatiMail@Mail.com",
            Password = "UnaContraBuena11."
        };
    }

    [TestMethod]
    public void CreateAVisitor_Constructor()
    {
        var visitor = new Visitor(_helper);

        Assert.IsNotNull(visitor);
    }

    [TestMethod]
    public void CreateAVisitor_WhitEmptyConstructor_returnVisitor()
    {
        var visitor = new Visitor();

        Assert.IsNotNull(visitor);
    }

    [TestMethod]
    public void SetVisitorName_GetCorrectly()
    {
        var visitor = new Visitor(_helper) { Name = "Mati" };

        Assert.AreEqual(visitor.Name, "Mati");
    }

    [TestMethod]
    public void SetVisitorEmail_GetCorrectly()
    {
        var visitor = new Visitor(_helper) { Email = "pantufla@mail.com" };

        Assert.AreEqual(visitor.Email, "pantufla@mail.com");
    }

    [TestMethod]
    public void SetVisitorPassword_GetCorrectly()
    {
        var visitor = new Visitor(_helper) { Password = "pantuflaConDulce" };

        Assert.AreEqual(visitor.Password, "pantuflaConDulce");
    }

    [TestMethod]
    public void SetVisitorBirthdate_GetCorrectly()
    {
        var visitor = new Visitor(_helper) { BirthDate = DateTime.Parse("10/07/2004") };

        Assert.AreEqual(visitor.BirthDate, DateTime.Parse("10/07/2004"));
    }

    [TestMethod]
    public void SetVisitorId_SetCorrectly()
    {
        var visitor = new Visitor(_helper) { Id = 0 };

        Assert.AreEqual(visitor.Id, 0);
    }

    [TestMethod]
    public void SetVisitorParkInformation_GetNotNull()
    {
        var visitor = new Visitor(_helper)
        {
            VisitorParkInformation = new VisitorParkInformation()
        };
        Assert.IsNotNull(visitor.VisitorParkInformation);
    }

    [TestMethod]
    public void GetIdentifierNFC()
    {
        var visitor = new Visitor(_helper);

        var result = visitor.GetIdentifierNFC();

        Assert.IsNotNull(result);
    }
}
