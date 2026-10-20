using Abstraction;
using Domain;

namespace DataAccess;

public class TicketRepository(SqlContext context) : Repository<Ticket>(context), ITicketRepository
{
    public Ticket? GetTicketByQr(Guid qr)
    {
        return Query().FirstOrDefault(t => t.QR.Equals(qr));
    }

    public void ConsumeTicket(Ticket ticket)
    {
        ticket.Consumed = true;
        Update(ticket);
    }
}
