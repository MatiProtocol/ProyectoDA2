using Abstraction;
using DataAccess;
using Domain;

namespace Tests.RepositoriesTest;
[TestClass]
public class AttractionRepositoryTest
{
    private IAttractionRepository _attractionRepository = null!;
    private SqlContext _context = null!;
    private Attraction _attraction = null!;
    private Visitor _visitor = null!;
    private Incidence _incidence = null!;
    private VisitorParkInformation _visitorParkInfo = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _attraction = new Attraction
        {
            CurrentCapacity = 1,
            Description = "Descricri",
            Id = 1,
            Incidences = [],
            MaxCapacity = 4,
            MinAge = 3,
            Name = "La cobra",
            TypeAttraction = TypeAttraction.RollerCoaster
        };

        _incidence = new Incidence
        {
            Id = 1,
            Active = false,
            AttractionName = _attraction.Name,
            Description = "Descricri",
            AttractionId = 1
        };
        _attraction.Incidences = [_incidence];

        _visitorParkInfo = new VisitorParkInformation()
        {
            Id = 1,
            NFC = Guid.Empty,
            Points = 0,
            Tickets = [],
            VisitorMembership = nameof(Membership.Standard),
            VisitedAttractions = []
        };

        _visitor = new Visitor
        {
            BirthDate = DateTime.Now,
            Email = "test@test.com",
            Id = 1,
            Name = "Davo",
            Password = "enter@2asF",
            VisitorParkInformation = _visitorParkInfo
        };

        _context.Attractions.Add(_attraction);
        _context.Visitors.Add(_visitor);
        _context.SaveChanges();

        _attractionRepository = new AttractionRepository(_context);
    }

    [TestMethod]
    public void CrateAttractionRepository_IsNotNull()
    {
        Assert.IsNotNull(_attractionRepository);
    }

    [TestMethod]
    public void GetByType_IsNotNull()
    {
        var repo = new AttractionRepository(_context);

        var result = repo.GetByType(TypeAttraction.RollerCoaster);
        Assert.AreEqual(result.Count(), 1);
    }

    [TestMethod]
    public void GetAvailableAttractions_IsNotNull()
    {
        var result = _attractionRepository.GetAvailableAttractions();
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetAttractionsWithoutActiveIncidences_IsNotNull()
    {
        var result = _attractionRepository.GetAttractionsWithoutActiveIncidences();
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void CreateVisitedAttraction_CreateCorrectly()
    {
        var visited = new VisitedAttraction
        {
            AttractionId = _attraction.Id,
            VisitorParkInformationId = _visitorParkInfo.Id,
            VisitDate = DateTime.Now
        };

        _attractionRepository.CreateVisitedAttraction(visited);

        var result = _context.VisitedAttractions.FirstOrDefault();
        Assert.AreEqual(_attraction.Id, result!.AttractionId);
    }

    [TestMethod]
    public void ExistActiveIncident_ReturnFalse()
    {
        var result = _attractionRepository.ExistIncidentActive(_attraction.Id);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ExistActiveIncident_ReturnTrue()
    {
        var incidence = new Incidence
        {
            Description = "Falla técnica",
            Active = true,
            AttractionId = _attraction.Id,
        };

        _context.Incidences.Add(incidence);
        _context.SaveChanges();

        var result = _attractionRepository.ExistIncidentActive(_attraction.Id);

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetAttractionByName_IsNotNull()
    {
        var result = _attractionRepository.GetAttractionByName("La cobra");

        Assert.IsNotNull(result);
    }
}
