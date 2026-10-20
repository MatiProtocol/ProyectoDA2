using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IIncidenceService : ICrudService<Incidence>
{
    public IEnumerable<Incidence> GetActiveIncidences();
    public IEnumerable<Incidence> GetByAttractionId(int attractionId);
    public void DeactivateIncidence(int id);
    public void ActivateIncidence(int id);
}
