using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface ITicketRepository : INonUserRepository<Ticket>
{
    public Ticket? GetTicketByQr(Guid qr);
    public void ConsumeTicket(Ticket ticket);
}
