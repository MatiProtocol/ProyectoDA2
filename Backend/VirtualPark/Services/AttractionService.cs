using Abstraction;
using Domain;
using Exceptions;
using Microsoft.IdentityModel.Tokens;
using NonDomainAbstractions;

namespace Services;

public class AttractionService(
    IAttractionRepository rp,
    ITicketService ts,
    IDateTimeService dt,
    IVisitorService vs,
    ISpecialEventService ses)
    : IAttractionService
{
    private readonly IAttractionRepository _repository = rp;
    private readonly ITicketService _ticketService = ts;
    private readonly IDateTimeService _dateTimeService = dt;
    private readonly IVisitorService _visitorService = vs;
    private readonly ISpecialEventService _specialEventService = ses;

    public Attraction GetById(int id)
    {
        return GetAux(id);
    }

    public int GetVisitsBetween(int attractionId, DateTime startDate, DateTime endDate)
    {
        return _visitorService.GetVisitsBetween(attractionId, startDate, endDate);
    }

    public void Create(Attraction entity)
    {
        ValidateAttraction(entity);

        if(_repository.GetAttractionByName(entity.Name) != null)
        {
            throw new DbCreatedUniqueOrKeyException("The attraction already exists");
        }

        _repository.Add(entity);
    }

    public IEnumerable<Attraction> GetAll()
    {
        return _repository.GetAll();
    }

    public void Update(Attraction entity)
    {
        ValidateAttraction(entity);
        _repository.Update(entity);
    }

    public void Delete(int id)
    {
        if(id <= 0)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException($"No attraction found for id: " + id, "Id"));
        }

        var attraction = _repository.GetById(id);

        if(attraction == null)
        {
            return;
        }

        _repository.Remove(attraction);
    }

    public IEnumerable<Attraction> GetByType(TypeAttraction type)
    {
        return _repository.GetByType(type);
    }

    public IEnumerable<Attraction> GetAvailableAttractions()
    {
        return _repository.GetAvailableAttractions();
    }

    public IEnumerable<Attraction> GetAttractionsWithoutActiveIncidences()
    {
        return _repository.GetAttractionsWithoutActiveIncidences();
    }

    public bool IncrementAttractionCapacity(int attractionId)
    {
        var attraction = _repository.GetById(attractionId);
        if(attraction == null)
        {
            throw new ArgumentException($"No attraction found for id: {attractionId}", nameof(attractionId));
        }

        var result = attraction.IncrementCapacity();
        if(result)
        {
            _repository.Update(attraction);
        }

        return result;
    }

    public void DecrementCurrentCapacity(int attractionId)
    {
        var attraction = _repository.GetById(attractionId);
        if(attraction == null)
        {
            throw new ArgumentException($"No attraction found for id: {attractionId}", nameof(attractionId));
        }

        var result = attraction.DecrementCapacity();
        if(result)
        {
            _repository.Update(attraction);
        }
    }

    public void ValidateAttractionAccess(int attractionId, Guid? ticketQr, Guid? visitorNfc)
    {
        if(_repository.ExistIncidentActive(attractionId))
        {
            throw new InvalidOperationException($"Attraction with id: {attractionId} have active/s incident/s");
        }

        var specialEvents = _specialEventService.GetActiveEvents()
                .Where(se => se.IncludedAttractions.Any(a => a.Id == attractionId)).ToList();

        Ticket? ticket = null;

        if(ticketQr.HasValue)
        {
            ticket = GetTicketByQr(ticketQr.Value, specialEvents);
        }

        if(ticket == null && visitorNfc.HasValue)
        {
            ticket = GetTicketByNfc(visitorNfc.Value, specialEvents);
        }

        if(ticket == null)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException("Ticket not found", "QR or Nfc"));
        }

        var visitor = _visitorService.GetById(ticket.VisitorParkInformationId);
        var attraction = GetById(attractionId);

        ValidateVisitor(visitor!, attraction);

        RegisterVisitedAttraction(attraction, ticket);
    }

    private Ticket? GetTicketByQr(Guid qr, List<SpecialEvent> specialEvents)
    {
        var ticket = _ticketService.GetTicketByQr(qr);

        if(ticket == null)
        {
            return null;
        }

        if(ticket.SpecialEventId is null)
        {
            return ticket;
        }

        return specialEvents.IsNullOrEmpty() || !specialEvents
                                                    .Exists(s =>
                                                        s.IncludedAttractions
                                                            .Exists(a => a.Id == ticket.SpecialEventId))
                                                                ? null : ticket;
    }

    private Ticket? GetTicketByNfc(Guid nfc, List<SpecialEvent> specialEvents)
    {
        var tickets = _visitorService.GetAvailableTicketsByNfc(nfc).ToList();

        if(tickets.Count == 0)
        {
            return null;
        }

        if(specialEvents.IsNullOrEmpty())
        {
            return tickets.FirstOrDefault(t => !t.SpecialEventId.HasValue);
        }

        var specialEventTicket = tickets
            .FirstOrDefault(t =>
                t.SpecialEventId.HasValue && specialEvents
                .Any(se => se.Id == t.SpecialEventId.Value));

        return specialEventTicket ?? tickets.FirstOrDefault(t => !t.SpecialEventId.HasValue);
    }

    private void ValidateVisitor(Visitor visitor, Attraction attraction)
    {
        var today = _dateTimeService.Now;
        var age = today.Year - visitor.BirthDate.Year;

        if(visitor.BirthDate.Date > today.AddYears(-age))
        {
            age--;
        }

        if(age < attraction.MinAge)
        {
            throw new MinimumAgeException($"Age must be greater than {attraction.MinAge} years");
        }
    }

    private void RegisterVisitedAttraction(Attraction attraction, Ticket ticket)
    {
        IncrementAttractionCapacity(attraction.Id);

        try
        {
            _ticketService.ConsumeTicket(ticket);
        }
        catch(Exception)
        {
            DecrementCurrentCapacity(attraction.Id);
            throw;
        }

        RegisterVisitedAttraction(ticket.VisitorParkInformationId, attraction.Id);
    }

    private void ValidateAttraction(Attraction entity)
    {
        if(string.IsNullOrWhiteSpace(entity.Name))
        {
            throw new ArgumentException("Attraction name cannot be null or empty", nameof(entity.Name));
        }

        if(entity.MaxCapacity <= 0)
        {
            throw new ArgumentException("Max capacity must be greater than 0", nameof(entity.MaxCapacity));
        }

        if(entity.MinAge < 0)
        {
            throw new ArgumentException("Minimum age cannot be negative", nameof(entity.MinAge));
        }
    }

    private Attraction GetAux(int id)
    {
        var attraction = _repository.GetById(id);
        if(attraction == null)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException($"No attraction found for id: " + id, "Id"));
        }

        return attraction;
    }

    private void RegisterVisitedAttraction(int visitorId, int attractionId)
    {
        var date = _dateTimeService.Now;

        var visitedAttraction = new VisitedAttraction(visitorId, attractionId, date);

        var visitorWithInfo = _visitorService.GetVisitorWithParkInformation(visitorId);
        visitorWithInfo.VisitorParkInformation.VisitedAttractions.Add(visitedAttraction);

        _visitorService.UpdateVisitorParkInformation(visitorWithInfo.VisitorParkInformation);
    }
}
