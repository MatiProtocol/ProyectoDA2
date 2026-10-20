using System.Globalization;
using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class VisitorDtoTests
{
    private Visitor _visitor = null!;
    private GetVisitorResponse _response = null!;

    [TestInitialize]
    public void Setup()
    {
        _visitor = new Visitor
        {
            Id = 1,
            Name = "Juan Pérez",
            Email = "juan@example.com",
            BirthDate = DateTime.Parse("2000/5/10")
        };

        _response = new GetVisitorResponse(_visitor);
    }

    [TestMethod]
    public void VisitorDTOIdGet_ReturnCorrectly()
    {
        Assert.AreEqual(_visitor.Id, _response.Id);
    }

    [TestMethod]
    public void VisitorDTONameGet_ReturnCorrectly()
    {
        Assert.AreEqual(_visitor.Name, _response.Name);
    }

    [TestMethod]
    public void VisitorDTOEmailGet_ReturnCorrectly()
    {
        Assert.AreEqual(_visitor.Email.ToString(), _response.Email);
    }

    [TestMethod]
    public void VisitorDTOBirthDateGet_ReturnCorrectly()
    {
        Assert.AreEqual(_visitor.BirthDate.ToString(CultureInfo.InvariantCulture), _response.BirthDate);
    }

    [TestMethod]
    public void VisitorDTOIdSet_AssignCorrectly()
    {
        _response.Id = 1;
        Assert.AreEqual(1, _response.Id);
    }

    [TestMethod]
    public void VisitorDTONameSet_AssignCorrectly()
    {
        _response.Name = "Pedro";
        Assert.AreEqual("Pedro", _response.Name);
    }

    [TestMethod]
    public void VisitorDTOEmailSet_AssignCorrectly()
    {
        _response.Email = "pedro@example.com";
        Assert.AreEqual("pedro@example.com", _response.Email);
    }

    [TestMethod]
    public void VisitorDTO_BirthDateSet_AssignCorrectly()
    {
        _response.BirthDate = "2025-09-15";
        Assert.AreEqual("2025-09-15", _response.BirthDate);
    }

    [TestMethod]
    public void SetMembership_GetCorrectly()
    {
        _response.Membership = nameof(Membership.Standard);

        Assert.AreEqual(nameof(Membership.Standard), _response.Membership);
    }

    [TestMethod]
    public void SetPoints_GetCorrectly()
    {
        _response.Points = 100;

        Assert.AreEqual(100, _response.Points);
    }

    [TestMethod]
    public void SetTodayPoints_GetCorrectly()
    {
        _response.TodayPoints = 50;

        Assert.AreEqual(50, _response.TodayPoints);
    }
}
