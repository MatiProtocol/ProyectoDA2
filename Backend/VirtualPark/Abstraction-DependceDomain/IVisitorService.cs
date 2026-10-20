using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IVisitorService : ICRUDUserService<Visitor>
{
    public Guid GetVisitorNFC(int id);
    public Visitor? GetByEmail(string email);
    public IQueryable<Ticket> GetAvailableTicketsByNfc(Guid nfc);
    public int GetVisitsBetween(int attractionId, DateTime startDate, DateTime endDate);
    public Visitor GetVisitorWithParkInformation(int id);
    public IQueryable<Ticket> GetAvailableTicketsById(int id);
    public void UpdateVisitorParkInformation(VisitorParkInformation visitorParkInformation);
}
