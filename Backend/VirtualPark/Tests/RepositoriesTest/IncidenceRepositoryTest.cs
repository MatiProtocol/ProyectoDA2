using Abstraction;
using DataAccess;
using Domain;
using NonDomainAbstractions;

namespace Tests.RepositoriesTest;

[TestClass]
public class IncidenceRepositoryTest
{
    private IncidenceRepository _incidenceRepository = null!;
    private SqlContext _context = null!;
    private Incidence _incidence = null!;
    private Incidence _incidenceInactive = null!;
    private Attraction _attraction1 = null!;
    private Attraction _attraction2 = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _attraction1 = new Attraction
        {
            CurrentCapacity = 1,
            Description = "Descricri1",
            Id = 1,
            Incidences = [],
            MaxCapacity = 4,
            MinAge = 3,
            Name = "La cobra",
            TypeAttraction = TypeAttraction.RollerCoaster
        };

        _attraction2 = new Attraction
        {
            CurrentCapacity = 0,
            Description = "Descricri2",
            Id = 2,
            Incidences = [],
            MaxCapacity = 4,
            MinAge = 3,
            Name = "La cobra2",
            TypeAttraction = TypeAttraction.RollerCoaster
        };

        _incidence = new Incidence
        {
            Id = 1,
            Description = "Test incidence active",
            AttractionId = 1,
            Active = true
        };
        _attraction1.Incidences = [_incidence];

        _incidenceInactive = new Incidence
        {
            Id = 2,
            Description = "Test incidence inactive",
            AttractionId = 2,
            Active = false
        };
        _attraction2.Incidences = [_incidenceInactive];

        _context.Attractions.AddRange(_attraction1, _attraction2);
        _context.SaveChanges();

        _incidenceRepository = new IncidenceRepository(_context);
    }

    [TestMethod]
    public void CreateIncidenceRepository_IsNotNull()
    {
        Assert.IsNotNull(_incidenceRepository);
    }

    [TestMethod]
    public void IncidenceRepository_ImplementsIIncidenceRepository()
    {
        Assert.IsInstanceOfType(_incidenceRepository, typeof(IIncidenceRepository));
    }

    [TestMethod]
    public void IncidenceRepository_ImplementsIRepository()
    {
        Assert.IsInstanceOfType(_incidenceRepository, typeof(IRepository<Incidence>));
    }

    [TestMethod]
    public void Add_CallsSaveChanges()
    {
        var newIncidence = new Incidence
        {
            Description = "New incidence",
            AttractionId = 2,
            Active = true,
            AttractionName = _attraction2.Name,
            Id = 3
        };

        _incidenceRepository.Add(newIncidence);

        var result = _context.Incidences.FirstOrDefault(i => i.Description == "New incidence");
        Assert.AreEqual(2, result!.AttractionId);
    }

    [TestMethod]
    public void Remove_RemovesFromDbSet_AndCallsSaveChanges()
    {
        _incidenceRepository.Remove(_incidence);

        var result = _context.Incidences.FirstOrDefault(i => i.Id == _incidence.Id);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void Update_UpdatesInDbSet_AndCallsSaveChanges()
    {
        _incidence.Description = "Updated description";
        _incidenceRepository.Update(_incidence);

        var result = _context.Incidences.FirstOrDefault(i => i.Id == _incidence.Id);
        Assert.AreEqual("Updated description", result!.Description);
    }

    [TestMethod]
    public void GetById_ReturnsCorrectIncidence()
    {
        var result = _incidenceRepository.GetById(1);
        Assert.AreEqual(_incidence.Id, result!.Id);
    }

    [TestMethod]
    public void GetById_ReturnsNull_WhenNotFound()
    {
        var result = _incidenceRepository.GetById(999);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetActiveIncidences_ReturnActiveIncidences()
    {
        var result = _incidenceRepository.GetActiveIncidences();
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetByAttractionId_ReturnsCorrectNumberOfIncidences()
    {
        _incidence = new Incidence
        {
            Id = 3,
            Description = "Test incidence active2",
            AttractionId = 1,
            Active = true
        };
        _context.Incidences.Add(_incidence);
        _context.SaveChanges();
        var result = _incidenceRepository.GetByAttractionId(1);
        Assert.AreEqual(2, result.Count());
    }
}
