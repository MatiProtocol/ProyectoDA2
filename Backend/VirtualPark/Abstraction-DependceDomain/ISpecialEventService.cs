using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface ISpecialEventService : ICrudService<SpecialEvent>
{
    void DeactivateEvent(int id);
    bool IsEventActive(int id, DateTime currentDateTime);
    void AddAttractionToEvent(int eventId, int attractionId);
    void RemoveAttractionFromEvent(int eventId, int attractionId);
    IEnumerable<SpecialEvent> GetActiveEvents();
    bool CanBuyEventTicket(int eventId);
    void IncrementEventCapacity(int eventId);
    void DecrementEventCapacity(int eventId);
    public bool AreAttractionPartOfAnyEvent(int attractionId);
    public void AddAttractions(int eventId, IEnumerable<int> attractions);
    public void AddAttractions(string eventName, IEnumerable<int> attractions);
}
