using Domain;

namespace Tests.DomainTest;
[TestClass]
public class VisitedAttractionTest
{
    private Visitor _visitor = null!;
    private Attraction _attraction = null!;
    private VisitedAttraction _visitedAttraction = null!;

    [TestInitialize]
    public void Initialize()
    {
        Visitor.VisitorConstructHelper helper;
        helper = new Visitor.VisitorConstructHelper
        {
            Name = "Ramon",
            Birthdate = DateTime.Parse("10/10/2010"),
            Email = "ramon@mail.com",
            Password = "Pass!.1234"
        };
        _visitor = new Visitor(helper);
        _attraction = new Attraction();
        _visitedAttraction = new VisitedAttraction();
    }

    [TestMethod]
    public void CreateVisitedAttractionEmptyConstructor_IsNotNull()
    {
        Assert.IsNotNull(_visitedAttraction);
    }

    [TestMethod]
    public void CreateVisitedAttractionWithParams_IsNotNull()
    {
        var date = DateTime.Now;
        var visitedAttraction = new VisitedAttraction(1, 1, date);
        Assert.IsNotNull(visitedAttraction);
    }

    [TestMethod]
    public void SetVisitorId_GetCorrectly()
    {
        _visitedAttraction.VisitorParkInformationId = _visitor.VisitorParkInformation.Id;
        Assert.AreEqual(_visitedAttraction.VisitorParkInformationId, _visitor.VisitorParkInformation.Id);
    }

    [TestMethod]
    public void SetAttractionId_GetCorrectly()
    {
        _visitedAttraction.AttractionId = _attraction.Id;
        Assert.AreEqual(_visitedAttraction.AttractionId, _attraction.Id);
    }

    [TestMethod]
    public void SetVisitDate_GetCorrectly()
    {
        var date = DateTime.Today;
        _visitedAttraction.VisitDate = date;
        Assert.AreEqual(_visitedAttraction.VisitDate, date);
    }

    [TestMethod]
    public void SetIsSpecialEvent_GetCorrectly()
    {
        _visitedAttraction.IsSpecialEvent = true;
        Assert.IsTrue(_visitedAttraction.IsSpecialEvent);
    }

    [TestMethod]
    public void SetAttractionType_GetCorrectly()
    {
        _visitedAttraction.AttractionType = TypeAttraction.RollerCoaster;
        Assert.AreEqual(_visitedAttraction.AttractionType, TypeAttraction.RollerCoaster);
    }

    [TestMethod]
    public void GetAttractionType_GetCorrectly()
    {
        _visitedAttraction.AttractionType = TypeAttraction.RollerCoaster;
        Assert.AreEqual(_visitedAttraction.GetAttractionType(), nameof(TypeAttraction.RollerCoaster));
    }
}
