using DataAccess;
using Domain;

namespace Tests.RepositoriesTest;

[TestClass]
public class RepositoryTest
{
    private Repository<Visitor> _repository = null!;
    private SqlContext _context = null!;
    private Visitor _visitor = null!;
    private VisitorParkInformation _visitorParkInfo = null!;

    [TestInitialize]
    public void Initialize()
    {
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

        _context = SqlContextInMemoryCreate.Create();
        _context.Visitors.Add(_visitor);
        _context.SaveChanges();

        _repository = new Repository<Visitor>(_context);
    }

    [TestMethod]
    public void CreateRepository_IsNotNull()
    {
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public void Add_SaveChanges()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            Id = 2,
            NFC = Guid.Empty,
            Points = 0,
            Tickets = [],
            VisitorMembership = nameof(Membership.Standard),
            VisitedAttractions = []
        };

        var visitor = new Visitor
        {
            Name = "Test",
            Password = "enter@2asF",
            VisitorParkInformation = visitorParkInformation,
            Id = 2,
            BirthDate = new DateTime(2000, 1, 1),
            Email = "test@2test.com"
        };

        _repository.Add(visitor);

        var result = _context.Visitors.FirstOrDefault(v => v.Name == "Test");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void Remove_SaveChanges()
    {
        _repository.Remove(_visitor);

        var result = _context.Visitors.FirstOrDefault(v => v.Name == "ToRemove");

        Assert.IsNull(result);
    }

    [TestMethod]
    public void Update_SaveChanges()
    {
        _visitor.Name = "NewName";
        _repository.Update(_visitor);

        var result = _context.Visitors.FirstOrDefault(v => v.Id == _visitor.Id);

        Assert.AreEqual("NewName", result!.Name);
    }

    [TestMethod]
    public void GetById_IsNotNull()
    {
        var result = _repository.GetById(_visitor.Id);

        Assert.AreEqual("Davo", result!.Name);
    }

    [TestMethod]
    public void GetAll_IsNotNull()
    {
        var visitorParkInformation = new VisitorParkInformation
        {
            Id = 2,
            NFC = Guid.Empty,
            Points = 0,
            Tickets = [],
            VisitorMembership = nameof(Membership.Standard),
            VisitedAttractions = []
        };

        var visitor = new Visitor
        {
            Name = "Test",
            Password = "enter@2asF",
            VisitorParkInformation = visitorParkInformation,
            Id = 2,
            BirthDate = new DateTime(2000, 1, 1),
            Email = "test@2test.com"
        };

        _context.Visitors.Add(visitor);
        _context.SaveChanges();

        var result = _repository.GetAll();
        Assert.AreEqual(2, result.Count());
    }

    [TestMethod]
    public void Query_returnCorrectly()
    {
        var count = _repository.Query().Count(v => v.Name == "Davo");
        Assert.AreEqual(1, count);
    }
}
