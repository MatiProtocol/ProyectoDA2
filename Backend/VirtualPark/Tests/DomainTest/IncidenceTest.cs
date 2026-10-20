using Domain;

namespace Tests.DomainTest;

[TestClass]
public class IncidenceTest
{
    private Incidence _incidence = null!;
    private Attraction _attraction = null!;

    [TestInitialize]
    public void Initialize()
    {
        _attraction = new Attraction
        {
            Name = "Dragon Coaster",
            TypeAttraction = TypeAttraction.RollerCoaster
        };

        _incidence = new Incidence
        {
            Id = 1,
            Description = "Mantenimiento preventivo",
            Active = true,
            AttractionId = 1,
            AttractionName = _attraction.Name
        };
    }

    [TestMethod]
    public void CreateIncidence_NotNull()
    {
        Assert.IsNotNull(_incidence);
    }

    [TestMethod]
    public void CreateIncidence_WithEmptyConstructor_NotNull()
    {
        var incidence = new Incidence();
        Assert.IsNotNull(incidence);
    }

    [TestMethod]
    public void SetIncidenceId_GetCorrectly()
    {
        _incidence.Id = 5;
        Assert.AreEqual(5, _incidence.Id);
    }

    [TestMethod]
    public void SetIncidenceDescription_GetCorrectly()
    {
        _incidence.Description = "Falla mecánica";
        Assert.AreEqual("Falla mecánica", _incidence.Description);
    }

    [TestMethod]
    public void SetIncidenceActive_GetCorrectly()
    {
        _incidence.Active = false;
        Assert.IsFalse(_incidence.Active);
    }

    [TestMethod]
    public void NewIncidence_DefaultActiveStatus_IsFalse()
    {
        var incidence = new Incidence();
        Assert.IsFalse(incidence.Active);
    }

    [TestMethod]
    public void CreateIncidence_Description_IsNotNull()
    {
        var incidence = new Incidence();
        Assert.IsNotNull(incidence.Description);
    }

    [TestMethod]
    public void CreateIncidence_Description_IsEmptyString()
    {
        var incidence = new Incidence();
        Assert.AreEqual(string.Empty, incidence.Description);
    }

    [TestMethod]
    public void SetIncidenceAttractionId_GetCorrectly()
    {
        _incidence.AttractionId = 10;
        Assert.AreEqual(10, _incidence.AttractionId);
    }

    [TestMethod]
    public void SetIncidenceAttraction_SetsCorrectly()
    {
        var newAttraction = new Attraction { Name = "New Attraction" };
        _incidence.AttractionName = newAttraction.Name;
        Assert.AreEqual(newAttraction.Name, _incidence.AttractionName);
    }

    [TestMethod]
    public void SetIncidenceAttraction_CanAccessAttractionName()
    {
        var newAttraction = new Attraction { Name = "New Attraction" };
        _incidence.AttractionName = newAttraction.Name;
        Assert.AreEqual("New Attraction", _incidence.AttractionName);
    }

    [TestMethod]
    public void SetIncidenceStartDate_GetCorrectly()
    {
        _incidence.StartDate = DateTime.Today;
        Assert.AreEqual(_incidence.StartDate, DateTime.Today);
    }

    [TestMethod]
    public void SetIsMaintenance_GetCorrectly()
    {
        _incidence.IsMaintenance = true;
        Assert.AreEqual(_incidence.IsMaintenance, true);
    }

    [TestMethod]
    public void SetIncidenceStartTime_GetCorrectly()
    {
        var time = DateTime.Now.TimeOfDay;
        _incidence.StartTime = time;
        Assert.AreEqual(_incidence.StartTime, time);
    }

    [TestMethod]
    public void SetMaintenanceDuration_GetCorrectly()
    {
        _incidence.Duration = 10;
        Assert.AreEqual(_incidence.Duration, 10);
    }
}
