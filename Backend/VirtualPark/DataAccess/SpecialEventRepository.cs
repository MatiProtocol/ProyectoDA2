using Abstraction;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class SpecialEventRepository(SqlContext context) : Repository<SpecialEvent>(context), ISpecialEventRepository
{
    public override SpecialEvent? GetById(int id)
    {
        return Query()
            .Where(e => e.Id == id)
            .Include(e => e.IncludedAttractions)
            .FirstOrDefault();
    }

    public SpecialEvent? GetByIdWithAttractions(int id)
    {
        return Query()
            .Where(se => se.Id == id)
            .Include(se => se.IncludedAttractions)
            .FirstOrDefault();
    }

    public IEnumerable<SpecialEvent> GetActiveEvents()
    {
        return Query()
            .Where(e => e.Active)
            .Include(e => e.IncludedAttractions)
            .ToList();
    }

    public IEnumerable<SpecialEvent> GetEventsByDate(DateTime date)
    {
        return Query()
            .Where(e => e.Date.Date == date.Date)
            .ToList();
    }

    public bool HasCapacityAvailable(int eventId)
    {
        var specialEvent = GetById(eventId);
        return specialEvent != null && specialEvent.IsCapacityAvailable();
    }

    public bool AreAttractionPartOfAnyEvent(int attractionId)
    {
        return Query().Any(se => se.IncludedAttractions.Any(a => a.Id == attractionId) && se.Active);
    }
}
