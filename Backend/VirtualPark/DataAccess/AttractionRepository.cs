using Abstraction;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class AttractionRepository(SqlContext context) : Repository<Attraction>(context), IAttractionRepository
{
    private DbSet<VisitedAttraction> VisitedTicketDbSet { get; } = context.Set<VisitedAttraction>();
    public IEnumerable<Attraction> GetByType(TypeAttraction type)
    {
        return Query().Where(a => a.TypeAttraction == type);
    }

    public IEnumerable<Attraction> GetAvailableAttractions()
    {
        return Query().Where(a => a.CurrentCapacity < a.MaxCapacity);
    }

    public IEnumerable<Attraction> GetAttractionsWithoutActiveIncidences()
    {
        return Query().Where(a => !a.HasActiveIncidence());
    }

    public void CreateVisitedAttraction(VisitedAttraction visitedAttraction)
    {
        VisitedTicketDbSet.Add(visitedAttraction);
        Context.SaveChanges();
    }

    public bool ExistIncidentActive(int id)
    {
        return Query().Any(a => a.Id == id && a.Incidences.Any(i => i.Active));
    }

    public Attraction? GetAttractionByName(string name)
    {
        return Query().FirstOrDefault(a => name.Equals(a.Name));
    }
}
