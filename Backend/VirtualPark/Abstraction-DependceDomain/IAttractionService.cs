using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IAttractionService : ICrudService<Attraction>
{
    public bool IncrementAttractionCapacity(int attractionId);
    public IEnumerable<Attraction> GetAttractionsWithoutActiveIncidences();
    public IEnumerable<Attraction> GetAvailableAttractions();
    public IEnumerable<Attraction> GetByType(TypeAttraction type);
    public void DecrementCurrentCapacity(int attractionId);
    public int GetVisitsBetween(int attractionId, DateTime startDate, DateTime endDate);
    public void ValidateAttractionAccess(int attractionId, Guid? ticketQr, Guid? visitorNfc);
}
