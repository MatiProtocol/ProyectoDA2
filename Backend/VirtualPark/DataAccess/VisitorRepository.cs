using Abstraction;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class VisitorRepository(SqlContext context) : Repository<Visitor>(context), IVisitorRepository
{
    private DbSet<VisitorParkInformation> VisitorParkInformationDbSet { get; } = context.Set<VisitorParkInformation>();
    public Visitor? GetByEmail(string email)
    {
        return DbSet.FirstOrDefault(w => w.Email == email);
    }

    public Visitor? GetVisitorWithParkInfo(int id)
    {
        return Query()
            .Where(v => v.Id == id)
            .Include(v => v.VisitorParkInformation)
            .ThenInclude(vpi => vpi.Tickets)
            .Include(v => v.VisitorParkInformation)
            .ThenInclude(vpi => vpi.VisitedAttractions)
            .Include(vpi => vpi.VisitorParkInformation)
            .ThenInclude(vpi => vpi.RewardRedemptions)
            .Include(vpi => vpi.VisitorParkInformation)
            .ThenInclude(vpi => vpi.PointsHistory)
            .SingleOrDefault();
    }

    public IQueryable<Ticket> GetAvailableTicketsByNfc(Guid nfc)
    {
        return Query()
            .Where(v => v.VisitorParkInformation.NFC == nfc)
            .SelectMany(v => v.VisitorParkInformation.Tickets).Where(t => !t.Consumed);
    }

    public int GetVisitsBetween(int id, DateTime startDate, DateTime endDate)
    {
        return Query()
            .SelectMany(visitor => visitor.VisitorParkInformation.VisitedAttractions)
            .Count(va => va.AttractionId == id &&
                         va.VisitDate >= startDate &&
                         va.VisitDate <= endDate);
    }

    public void Update(Visitor entity, bool changePassword = false)
    {
        var existing = Context.Visitors.Find(entity.Id);
        existing!.Name = entity.Name;
        existing.Email = entity.Email;
        existing.BirthDate = entity.BirthDate;

        if(changePassword)
        {
            existing.Password = entity.Password;
        }

        Context.Entry(entity.VisitorParkInformation).State = EntityState.Unchanged;
        Context.SaveChanges();
    }

    public void UpdateVisitorParkInformation(VisitorParkInformation visitorParkInformation)
    {
        VisitorParkInformationDbSet.Update(visitorParkInformation);
        Context.SaveChanges();
    }

    public IQueryable<Ticket> GetAvailableTicketsById(int id)
    {
        return Query()
            .Where(v => v.Id == id)
            .SelectMany(v => v.VisitorParkInformation.Tickets).Where(t => !t.Consumed);
    }
}
