using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface ISpecialEventRepository : INonUserRepository<SpecialEvent>
{
    SpecialEvent? GetByIdWithAttractions(int id);
    IEnumerable<SpecialEvent> GetActiveEvents();
    IEnumerable<SpecialEvent> GetEventsByDate(DateTime date);
    bool HasCapacityAvailable(int eventId);
    public bool AreAttractionPartOfAnyEvent(int attractionId);
}
