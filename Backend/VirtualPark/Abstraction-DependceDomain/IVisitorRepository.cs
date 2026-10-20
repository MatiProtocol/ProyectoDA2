using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IVisitorRepository : IUserRepository<Visitor>
{
    public Visitor? GetByEmail(string email);
    public Visitor? GetVisitorWithParkInfo(int id);
    public IQueryable<Ticket> GetAvailableTicketsByNfc(Guid nfc);
    public int GetVisitsBetween(int id, DateTime startDate, DateTime endDate);
    public IQueryable<Ticket> GetAvailableTicketsById(int id);
    public void UpdateVisitorParkInformation(VisitorParkInformation visitorParkInformation);
}
