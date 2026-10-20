using Abstraction;
using DataAccess;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Tests.RepositoriesTest;

[TestClass]
public class VisitorRepositoryTest
{
    private IVisitorRepository _visitorRepository = null!;
    private SqlContext _context = null!;
    private Visitor _visitor = null!;
    private VisitorParkInformation _visitorParkInformation = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _visitorParkInformation = new VisitorParkInformation
        {
            Id = 1,
            NFC = Guid.NewGuid(),
            Tickets = [],
            VisitedAttractions = [],
            VisitorMembership = nameof(Membership.Standard),
            Points = 0
        };

        _visitor = new Visitor
        {
            Name = "pepe",
            Password = "asdasd@12",
            Email = "asda@asd.com",
            BirthDate = DateTime.Now,
            VisitorParkInformation = _visitorParkInformation,
        };

        _context.Visitors.Add(_visitor);
        _context.SaveChanges();

        _visitorRepository = new VisitorRepository(_context);
    }

    [TestMethod]
    public void VisitorRepository_IsNotNull()
    {
        Assert.IsNotNull(_visitorRepository);
    }

    [TestMethod]
    public void GetByEmail_IsNotNull()
    {
        var result = _visitorRepository.GetByEmail("asda@asd.com");

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetTicketsAvailableByNfc_GetCorrectly()
    {
        var ticket = new Ticket
        {
            QR = Guid.NewGuid(),
            Date = DateTime.Now,
            Type = nameof(TicketType),
            VisitorParkInformationId = _visitor.VisitorParkInformation.Id
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        var result = _visitorRepository.GetAvailableTicketsByNfc(_visitor.GetIdentifierNFC()).ToList();

        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetVisitsBetween_WithVisitsInRange_ReturnsCorrectCount()
    {
        var attractionId = 1;
        var startDate = DateTime.Parse("2024-01-01");
        var endDate = DateTime.Parse("2024-01-31");

        var attraction = new Attraction()
        {
            Id = attractionId,
            Incidences = [],
            CurrentCapacity = 1,
            Description = "descricri",
            MaxCapacity = 5,
            MinAge = 12,
            Name = "Pedro",
            TypeAttraction = TypeAttraction.Show
        };

        _context.Attractions.Add(attraction);
        _context.SaveChanges();

        var visitedAttractions = new List<VisitedAttraction>
        {
            new() { AttractionId = attractionId, VisitDate = DateTime.Parse("2024-01-15"), VisitorParkInformationId = _visitor.VisitorParkInformation.Id },
            new() { AttractionId = attractionId, VisitDate = DateTime.Parse("2024-01-20"), VisitorParkInformationId = _visitor.VisitorParkInformation.Id },
            new() { AttractionId = attractionId, VisitDate = DateTime.Parse("2023-12-31"), VisitorParkInformationId = _visitor.VisitorParkInformation.Id },
            new() { AttractionId = attractionId, VisitDate = DateTime.Parse("2024-02-01"), VisitorParkInformationId = _visitor.VisitorParkInformation.Id },
            new() { AttractionId = 2, VisitDate = DateTime.Parse("2024-01-10"), VisitorParkInformationId = _visitor.VisitorParkInformation.Id }
        };

        _context.AddRange(visitedAttractions);
        _context.SaveChanges();

        _visitor.VisitorParkInformation.VisitedAttractions = visitedAttractions;
        _context.SaveChanges();

        var result = _visitorRepository.GetVisitsBetween(attractionId, startDate, endDate);

        Assert.AreEqual(2, result);
    }

    [TestMethod]
    public void GetVisitorWithParkInfo_NotNull()
    {
        var result = _visitorRepository.GetVisitorWithParkInfo(_visitor.Id);

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetVisitorWithParkInfo_IncludesParkInformation()
    {
        _visitor.VisitorParkInformation = null!;
        _visitorParkInformation.Tickets = null!;
        _visitorParkInformation.VisitedAttractions = null!;
        _context.Entry(_visitor).State = EntityState.Detached;

        var result = _visitorRepository.GetVisitorWithParkInfo(_visitor.Id);

        Assert.IsNotNull(result!.VisitorParkInformation);
    }

    [TestMethod]
    public void GetVisitorWithParkInfo_ParkInformationIncludesTickets()
    {
        _visitor.VisitorParkInformation = null!;
        _visitorParkInformation.Tickets = null!;
        _visitorParkInformation.VisitedAttractions = null!;
        _context.Entry(_visitor).State = EntityState.Detached;

        var result = _visitorRepository.GetVisitorWithParkInfo(_visitor.Id);

        Assert.IsNotNull(result!.VisitorParkInformation.Tickets);
    }

    [TestMethod]
    public void GetVisitorWithParkInfo_ParkInformationIncludesAttractions()
    {
        _visitor.VisitorParkInformation = null!;
        _visitorParkInformation.Tickets = null!;
        _visitorParkInformation.VisitedAttractions = null!;
        _context.Entry(_visitor).State = EntityState.Detached;

        var result = _visitorRepository.GetVisitorWithParkInfo(_visitor.Id);

        Assert.IsNotNull(result!.VisitorParkInformation.VisitedAttractions);
    }

    [TestMethod]
    public void Update_UpdateCorrectly()
    {
        _visitor.Name = "Updated";
        _visitor.VisitorParkInformation.Points = 100;

        _visitorRepository.Update(_visitor, false);
        _context.Entry(_visitor).State = EntityState.Detached;

        var updatedVisitor = _context.Visitors
            .Include(v => v.VisitorParkInformation)
            .ThenInclude(vpi => vpi.Tickets)
            .Single(v => v.Id == _visitor.Id);

        Assert.AreEqual("Updated", updatedVisitor.Name);
    }

    [TestMethod]
    public void UpdateChangePassword_UpdateCorrectly()
    {
        _visitor.Name = "Updated";
        _visitor.VisitorParkInformation.Points = 100;

        _visitorRepository.Update(_visitor, true);
        _context.Entry(_visitor).State = EntityState.Detached;

        var updatedVisitor = _context.Visitors
            .Include(v => v.VisitorParkInformation)
            .ThenInclude(vpi => vpi.Tickets)
            .Single(v => v.Id == _visitor.Id);

        Assert.AreEqual("Updated", updatedVisitor.Name);
    }

    [TestMethod]
    public void UpdateVisitorParkInfo_UpdateCorrectly()
    {
        _visitor.VisitorParkInformation.Points = 100;

        _visitorRepository.UpdateVisitorParkInformation(_visitor.VisitorParkInformation);
        _context.Entry(_visitor).State = EntityState.Detached;

        var updatedVisitor = _context.Visitors
            .Include(v => v.VisitorParkInformation)
            .ThenInclude(vpi => vpi.Tickets)
            .Single(v => v.Id == _visitor.Id);

        Assert.AreEqual(100, updatedVisitor.VisitorParkInformation.Points);
    }

    [TestMethod]
    public void Update_NoUpdateVisitorParkInformation()
    {
        _visitor.Name = "Updated";
        _visitor.VisitorParkInformation.Points = 0;

        _visitorRepository.Update(_visitor, false);
        _context.Entry(_visitor).State = EntityState.Detached;

        var updatedVisitor = _context.Visitors
            .Include(v => v.VisitorParkInformation).Single();

        Assert.AreEqual(0, updatedVisitor.VisitorParkInformation.Points);
    }

    [TestMethod]
    public void GetAvailableTicketsById_GetCorrectly()
    {
        var ticket = new Ticket
        {
            QR = Guid.NewGuid(),
            Date = DateTime.Now,
            Type = nameof(TicketType),
            VisitorParkInformationId = _visitor.VisitorParkInformation.Id
        };

        _context.Tickets.Add(ticket);
        _context.SaveChanges();

        _visitorRepository.GetAvailableTicketsById(_visitor.Id);
    }
}
