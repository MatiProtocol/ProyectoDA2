using Abstraction;
using Domain;
using Exceptions;
using NonDomainAbstractions;

namespace Services;

public class SpecialEventService(
    ISpecialEventRepository repository,
    IAttractionRepository attractionRepo,
    IDateTimeService dateTimeService) : ISpecialEventService
{
    private readonly ISpecialEventRepository _repository = repository;
    private readonly IAttractionRepository _attractionRepo = attractionRepo;
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    public SpecialEvent GetById(int id)
    {
        var specialEvent = _repository.GetById(id);
        if(specialEvent == null)
        {
            throw new ArgumentNullException($"No special event found for id: {id}", new DbKeyDoesNotExistException());
        }

        return specialEvent;
    }

    public IEnumerable<SpecialEvent> GetAll()
    {
        var result = _repository.GetAll().Select(InjectAttractionsToReturn);
        return result;
    }

    private SpecialEvent InjectAttractionsToReturn(SpecialEvent sp)
    {
        return _repository.GetByIdWithAttractions(sp.Id)!;
    }

    public void Create(SpecialEvent entity)
    {
        ValidateSpecialEvent(entity);

        _repository.Add(entity);
    }

    public void Update(SpecialEvent entity)
    {
        ValidateSpecialEvent(entity);

        _repository.Update(entity);
    }

    public void Delete(int id)
    {
        var specialEvent = _repository.GetById(id);
        if(specialEvent == null)
        {
            throw new ArgumentException($"No special event found for id: {id}", nameof(id));
        }

        _repository.Remove(specialEvent);
    }

    public void DeactivateEvent(int id)
    {
        var specialEvent = GetById(id);
        specialEvent.Active = false;
        _repository.Update(specialEvent);
    }

    public bool IsEventActive(int id, DateTime currentDateTime)
    {
        var specialEvent = GetById(id);
        return specialEvent.IsEventActive(currentDateTime);
    }

    public void AddAttractionToEvent(int eventId, int attractionId)
    {
        var specialEvent = GetById(eventId);

        var attraction = _attractionRepo.GetById(attractionId);

        if(specialEvent.IncludedAttractions.Any(a => a.Id == attraction!.Id))
        {
            return;
        }

        specialEvent.IncludedAttractions.Add(attraction!);

        _repository.Update(specialEvent);
    }

    public void RemoveAttractionFromEvent(int eventId, int attractionId)
    {
        var specialEvent = GetById(eventId);
        specialEvent.IncludedAttractions
            .Remove(specialEvent.IncludedAttractions
                .First(a => a.Id == attractionId));

        _repository.Update(specialEvent);
    }

    public IEnumerable<SpecialEvent> GetActiveEvents()
    {
        return _repository.GetActiveEvents();
    }

    public bool CanBuyEventTicket(int eventId)
    {
        return _repository.HasCapacityAvailable(eventId);
    }

    public void IncrementEventCapacity(int eventId)
    {
        var specialEvent = GetById(eventId);
        if(!specialEvent.IncrementCapacity())
        {
            throw new InvalidOperationException($"Event {eventId} is at maximum capacity");
        }

        _repository.Update(specialEvent);
    }

    public void DecrementEventCapacity(int eventId)
    {
        var specialEvent = GetById(eventId);
        specialEvent.DecrementCapacity();

        _repository.Update(specialEvent);
    }

    private void ValidateSpecialEvent(SpecialEvent entity)
    {
        if(string.IsNullOrWhiteSpace(entity.Name))
        {
            throw new InvalidOperationException("Event name cannot be null or empty");
        }

        if(entity.MaxCapacity <= 0)
        {
            throw new InvalidOperationException("Max capacity must be greater than 0");
        }

        if(entity.AdditionalCost < 0)
        {
            throw new InvalidOperationException("Additional cost cannot be negative");
        }

        if(entity.StartTime >= entity.EndTime)
        {
            throw new InvalidOperationException("Start time must be before end time");
        }

        if(entity.Date.Date < _dateTimeService.Now.Date)
        {
            throw new InvalidOperationException("Event date cannot be in the past");
        }
    }

    public bool AreAttractionPartOfAnyEvent(int attractionId)
    {
        return _repository.AreAttractionPartOfAnyEvent(attractionId);
    }

    public void AddAttractions(int eventId, IEnumerable<int> attractions)
    {
        var specialEvent = GetById(eventId);

        foreach(var attractionId in attractions)
        {
            var attraction = _attractionRepo.GetById(attractionId);

            if(attraction == null)
            {
                throw new DbKeyDoesNotExistException($"The key {attractionId} does not exist");
            }

            if(specialEvent.IncludedAttractions.Any(a => a.Id == attraction.Id))
            {
                continue;
            }

            specialEvent.IncludedAttractions.Add(attraction);
        }

        _repository.Update(specialEvent);
    }

    public void AddAttractions(string eventName, IEnumerable<int> attractions)
    {
        var specialEvent = GetAll().FirstOrDefault(e => e.Name.Equals(eventName));
        if(specialEvent == null)
        {
            throw new DbKeyDoesNotExistException($"The key {eventName} does not exist");
        }

        AddAttractions(specialEvent.Id, attractions);
    }
}
