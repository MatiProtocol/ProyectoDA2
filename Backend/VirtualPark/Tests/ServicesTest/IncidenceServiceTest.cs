using Abstraction;
using Domain;
using Moq;
using NonDomainAbstractions;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class IncidenceServiceTest
{
    private Mock<IAttractionRepository> _attractionRepo = null!;
    private Mock<IIncidenceRepository> _incidenceRepo = null!;
    private IIncidenceService _service = null!;
    private Incidence _incidence = null!;
    private readonly Incidence? _nullIncidence = null;
    private Mock<IDateTimeService> _dateTimeService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _incidenceRepo = new Mock<IIncidenceRepository>(MockBehavior.Strict);
        _attractionRepo = new Mock<IAttractionRepository>(MockBehavior.Strict);
        _dateTimeService = new Mock<IDateTimeService>(MockBehavior.Strict);
        _dateTimeService.Setup(dt => dt.Now).Returns(DateTime.Today.AddHours(12));

        _service = new IncidenceService(_incidenceRepo.Object, _attractionRepo.Object, _dateTimeService.Object);
        _incidence = new Incidence
        {
            Id = 1,
            Description = "Test incidence",
            AttractionId = 1,
            Active = true
        };
    }

    [TestMethod]
    public void CreateIncidenceService_IsNotNull()
    {
        Assert.IsNotNull(_service);
    }

    [TestMethod]
    public void IncidenceService_ImplementsIService()
    {
        Assert.IsInstanceOfType(_service, typeof(IRUService<Incidence>));
    }

    [TestMethod]
    public void CreateIncidence_WithValidData_CallsRepositoryAdd()
    {
        _incidenceRepo.Setup(r => r.Add(It.IsAny<Incidence>()));
        _attractionRepo.Setup(r => r.GetById(1)).Returns(new Attraction());
        _service.Create(_incidence);
        _incidenceRepo.VerifyAll();
    }

    [TestMethod]
    public void CreateMaintenance_WithInValidStartTime_ThrowsArgumentException()
    {
        _incidenceRepo.Setup(r => r.Add(It.IsAny<Incidence>()));
        _attractionRepo.Setup(r => r.GetById(1)).Returns(new Attraction());

        _incidence.IsMaintenance = true;
        _incidence.Duration = 10;
        _incidence.StartDate = DateTime.Today;
        _incidence.StartTime = _dateTimeService.Object.Now.Date.AddHours(-13).TimeOfDay;

        Assert.ThrowsException<ArgumentException>(() => _service.Create(_incidence));
    }

    [TestMethod]
    public void CreateMaintenance_WithInValidStartDate_ThrowsArgumentException()
    {
        _incidenceRepo.Setup(r => r.Add(It.IsAny<Incidence>()));
        _attractionRepo.Setup(r => r.GetById(1)).Returns(new Attraction());

        _incidence.IsMaintenance = true;
        _incidence.Duration = 10;
        _incidence.StartDate = DateTime.Today.AddDays(-1);
        _incidence.StartTime = _dateTimeService.Object.Now.TimeOfDay;

        Assert.ThrowsException<ArgumentException>(() => _service.Create(_incidence));
    }

    [TestMethod]
    public void CreateMaintenance_WithInValidDuration_ThrowsArgumentException()
    {
        _incidenceRepo.Setup(r => r.Add(It.IsAny<Incidence>()));
        _attractionRepo.Setup(r => r.GetById(1)).Returns(new Attraction());

        _incidence.IsMaintenance = true;
        _incidence.Duration = 0;
        _incidence.StartDate = DateTime.Today;
        _incidence.StartTime = _dateTimeService.Object.Now.TimeOfDay;

        Assert.ThrowsException<ArgumentException>(() => _service.Create(_incidence));
    }

    [TestMethod]
    public void CreateIncidence_WithNullEntity_ThrowsException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => _service.Create(null!));
    }

    [TestMethod]
    public void CreateIncidence_WithNonTrackedAttraction_ThrowsException()
    {
        Attraction nullAttraction = null!;
        _incidenceRepo.Setup(r => r.Add(It.IsAny<Incidence>()));
        _attractionRepo.Setup(r => r.GetById(1)).Returns(nullAttraction);
        Assert.ThrowsException<InvalidOperationException>(() => _service.Create(_incidence));
    }

    [TestMethod]
    public void CreateIncidence_WithEmptyDescription_ThrowsException()
    {
        var invalidIncidence = new Incidence
        {
            Description = " ",
            AttractionId = 1
        };
        Assert.ThrowsException<ArgumentException>(() => _service.Create(invalidIncidence));
    }

    [TestMethod]
    public void CreateIncidence_WithWhitespaceDescription_ThrowsException()
    {
        var invalidIncidence = new Incidence
        {
            Description = "   ",
            AttractionId = 1
        };
        Assert.ThrowsException<ArgumentException>(() => _service.Create(invalidIncidence));
    }

    [TestMethod]
    public void CreateIncidence_WithInvalidAttractionId_ThrowsException()
    {
        var invalidIncidence = new Incidence
        {
            Description = "Valid description",
            AttractionId = 0
        };
        Assert.ThrowsException<ArgumentException>(() => _service.Create(invalidIncidence));
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsNotNull()
    {
        _incidenceRepo.Setup(r => r.GetById(_incidence.Id)).Returns(_incidence);
        var result = _service.GetById(_incidence.Id);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetById_WithExistingId_ReturnsCorrectIncidence()
    {
        _incidenceRepo.Setup(r => r.GetById(_incidence.Id)).Returns(_incidence);
        var result = _service.GetById(_incidence.Id);
        Assert.AreEqual(_incidence.Id, result?.Id);
    }

    [TestMethod]
    public void GetById_WithNonExistentId_ReturnsNull()
    {
        _incidenceRepo.Setup(r => r.GetById(999)).Returns(_nullIncidence);
        var result = _service.GetById(999);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetAll_ReturnsCorrectNumberOfIncidences()
    {
        var incidences = new List<Incidence> { _incidence, new Incidence() };
        _incidenceRepo.Setup(r => r.GetAll()).Returns(incidences);
        var result = _service.GetAll();
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void Update_WithValidData_CallsRepositoryUpdate()
    {
        _incidenceRepo.Setup(r => r.Update(It.IsAny<Incidence>()));
        _attractionRepo.Setup(r => r.GetById(1)).Returns(new Attraction());
        _service.Update(_incidence);
        _incidenceRepo.VerifyAll();
    }

    [TestMethod]
    public void UpdateIncidence_WithNonTrackedAttraction_ThrowsException()
    {
        Attraction nullAttraction = null!;
        _incidenceRepo.Setup(r => r.Update(It.IsAny<Incidence>()));
        _attractionRepo.Setup(r => r.GetById(1)).Returns(nullAttraction);
        Assert.ThrowsException<InvalidOperationException>(() => _service.Update(_incidence));
    }

    [TestMethod]
    public void Update_WithInvalidData_ThrowsException()
    {
        var invalidIncidence = new Incidence
        {
            Description = " ",
            AttractionId = 1
        };
        Assert.ThrowsException<ArgumentException>(() => _service.Update(invalidIncidence));
    }

    [TestMethod]
    public void Delete_WithExistingId_RemovesIncidence()
    {
        _incidenceRepo.Setup(r => r.GetById(_incidence.Id)).Returns(_incidence);
        _incidenceRepo.Setup(r => r.Remove(It.IsAny<Incidence>()));
        _service.Delete(_incidence.Id);
        _incidenceRepo.VerifyAll();
    }

    [TestMethod]
    public void Delete_WithNonExistentId_ThrowsExceptionWithCorrectMessage()
    {
        _incidenceRepo.Setup(r => r.GetById(999)).Returns(_nullIncidence);
        var exception = Assert.ThrowsException<ArgumentException>(() => _service.Delete(999));
        Assert.IsTrue(exception.Message.Contains("No incidence found"));
    }

    [TestMethod]
    public void GetActiveIncidences_ReturnsCorrectNumberOfActiveIncidences()
    {
        var incidences = new List<Incidence>
        {
            new Incidence { Id = 1, Active = true },
            new Incidence { Id = 3, Active = true }
        }.AsQueryable();
        _incidenceRepo.Setup(r => r.GetActiveIncidences()).Returns(incidences);
        var result = _service.GetActiveIncidences();
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetActiveIncidences_ReturnsOnlyActiveIncidences()
    {
        var incidences = new List<Incidence>
        {
            new Incidence { Id = 1, Active = true },
            new Incidence { Id = 3, Active = true }
        }.AsQueryable();
        _incidenceRepo.Setup(r => r.GetActiveIncidences()).Returns(incidences);
        var result = _service.GetActiveIncidences();
        Assert.IsTrue(result.All(i => i.Active));
    }

    [TestMethod]
    public void GetByAttractionId_ReturnsCorrectNumberOfIncidences()
    {
        var incidences = new List<Incidence>
        {
            new Incidence { Id = 1, AttractionId = 1 },
            new Incidence { Id = 3, AttractionId = 1 }
        }.AsQueryable();
        _incidenceRepo.Setup(r => r.GetByAttractionId(1)).Returns(incidences);
        var result = _service.GetByAttractionId(1);
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void GetByAttractionId_ReturnsOnlyIncidencesForSpecifiedAttraction()
    {
        var incidences = new List<Incidence>
        {
            new Incidence { Id = 1, AttractionId = 1 },
            new Incidence { Id = 3, AttractionId = 1 }
        }.AsQueryable();
        _incidenceRepo.Setup(r => r.GetByAttractionId(1)).Returns(incidences);
        var result = _service.GetByAttractionId(1);
        Assert.IsTrue(result.All(i => i.AttractionId == 1));
    }

    [TestMethod]
    public void DeactivateIncidence_WithExistingId_SetsActiveToFalse()
    {
        var activeIncidence = new Incidence
        {
            Id = 1,
            Description = "Test",
            Active = true,
            AttractionId = 1
        };
        _incidenceRepo.Setup(r => r.GetById(activeIncidence.Id)).Returns(activeIncidence);
        _incidenceRepo.Setup(r => r.Update(It.IsAny<Incidence>()));
        _service.DeactivateIncidence(activeIncidence.Id);
        Assert.IsFalse(activeIncidence.Active);
    }

    [TestMethod]
    public void DeactivateIncidence_WithNonExistentId_ThrowsException()
    {
        _incidenceRepo.Setup(r => r.GetById(999)).Returns(_nullIncidence);
        Assert.ThrowsException<ArgumentException>(() => _service.DeactivateIncidence(999));
    }

    [TestMethod]
    public void ActivateIncidence_WithExistingId_SetsActiveToTrue()
    {
        var inactiveIncidence = new Incidence
        {
            Id = 2,
            Description = "Test",
            Active = false,
            AttractionId = 1
        };
        _incidenceRepo.Setup(r => r.GetById(inactiveIncidence.Id)).Returns(inactiveIncidence);
        _incidenceRepo.Setup(r => r.Update(It.IsAny<Incidence>()));
        _service.ActivateIncidence(inactiveIncidence.Id);
        Assert.IsTrue(inactiveIncidence.Active);
    }

    [TestMethod]
    public void ActivateIncidence_WithNonExistentId_ThrowsException()
    {
        _incidenceRepo.Setup(r => r.GetById(999)).Returns(_nullIncidence);
        Assert.ThrowsException<ArgumentException>(() => _service.ActivateIncidence(999));
    }
}
