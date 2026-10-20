using DataAccess;
using Domain;

namespace Tests.RepositoriesTest;

[TestClass]
public class TicketRepositoryTest
{
    private SqlContext _context = null!;
    private TicketRepository _ticketRepository = null!;
    private Ticket _ticket = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _ticket = new Ticket
        {
            Date = DateTime.Now,
            QR = Guid.Empty,
            Type = nameof(TicketType),
            VisitorParkInformationId = 1
        };

        _context.Tickets.Add(_ticket);
        _context.SaveChanges();

        _ticketRepository = new TicketRepository(_context);
    }

    [TestMethod]
    public void TicketRepository_IsNotNull()
    {
        Assert.IsNotNull(_ticketRepository);
    }

    [TestMethod]
    public void GetByQr_GetCorrectly()
    {
        var qr = Guid.Empty;

        var ticket = _ticketRepository.GetTicketByQr(qr);

        Assert.IsNotNull(ticket);
    }

    [TestMethod]
    public void ConsumeTicket_ChangeConsumeState()
    {
        _ticketRepository.ConsumeTicket(_ticket);

        Assert.IsTrue(_ticket.Consumed);
    }
}
