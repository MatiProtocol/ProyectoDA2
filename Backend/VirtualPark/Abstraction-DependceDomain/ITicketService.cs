using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface ITicketService : ICrudService<Ticket>
{
    public void ConsumeTicket(Ticket ticket);
    public Ticket? GetTicketByQr(Guid qr);
    public void BuyTicket(int visitorId, int eventId = 0);
}
