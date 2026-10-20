using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IIncidenceRepository : INonUserRepository<Incidence>
{
    public IEnumerable<Incidence> GetActiveIncidences();
    public IEnumerable<Incidence> GetByAttractionId(int attractionId);
}
