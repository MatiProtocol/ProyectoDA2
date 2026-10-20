using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IAttractionRepository : INonUserRepository<Attraction>
{
    public IEnumerable<Attraction> GetByType(TypeAttraction type);
    public IEnumerable<Attraction> GetAvailableAttractions();
    public IEnumerable<Attraction> GetAttractionsWithoutActiveIncidences();
    public void CreateVisitedAttraction(VisitedAttraction visitedAttraction);
    public bool ExistIncidentActive(int id);
    public Attraction? GetAttractionByName(string name);
}
