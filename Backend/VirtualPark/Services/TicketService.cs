using Abstraction;
using Domain;
using Exceptions;
using NonDomainAbstractions;

namespace Services;

public class TicketService(ITicketRepository rp, IDateTimeService dt, IVisitorService vs, ISpecialEventService ses) : ITicketService
{
    private readonly ITicketRepository _repository = rp;
    private readonly IDateTimeService _dateTimeService = dt;
    private readonly IVisitorService _visitorService = vs;
    private readonly ISpecialEventService _ses = ses;

    public Ticket? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public IEnumerable<Ticket> GetAll()
    {
        return _repository.GetAll();
    }

    public void Create(Ticket entity)
    {
        var currentTime = _dateTimeService.Now;
        if(currentTime.Hour >= 21)
        {
            entity.Date = currentTime.Date.AddDays(1);
        }
        else
        {
            entity.Date = currentTime;
        }

        _repository.Add(entity);
    }

    public void Update(Ticket entity)
    {
        _repository.Update(entity);
    }

    public void Delete(int id)
    {
        var ticket = GetById(id);
        _repository.Remove(ticket!);
    }

    public void ConsumeTicket(Ticket ticket)
    {
        if(ticket.Consumed)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException("Ticket already consumed", nameof(ticket)));
        }

        _repository.ConsumeTicket(ticket);
    }

    public Ticket GetTicketByQr(Guid qr)
    {
        return _repository.GetTicketByQr(qr) ?? throw new DbKeyDoesNotExistException("The ticket does not exist");
    }

    public void BuyTicket(int visitorId, int eventId = 0)
    {
        if(eventId == 0)
        {
            BuyGeneralTicket(visitorId);
        }
        else
        {
            BuySpecialEventTicket(visitorId, eventId);
        }
    }

    private void BuyGeneralTicket(int visitorId)
    {
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);

        if(visitor == null)
        {
            throw new ArgumentNullException("The visitor does not exist", new DbKeyDoesNotExistException());
        }

        var ticket = new Ticket
        {
            VisitorParkInformationId = visitor.VisitorParkInformation.Id,
            Type = nameof(TicketType.General),
            SpecialEventId = null,
            Date = _dateTimeService.Now
        };

        _repository.Add(ticket);
    }

    private void BuySpecialEventTicket(int visitorId, int eventId)
    {
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);
        if(visitor == null)
        {
            throw new DbKeyDoesNotExistException("The visitor does not exist");
        }

        var specialEvent = _ses.GetById(eventId);

        if(!specialEvent!.Active)
        {
            throw new InvalidOperationException("Event is not active");
        }

        if(!_ses.CanBuyEventTicket(eventId))
        {
            throw new InvalidOperationException("Event has no capacity available");
        }

        if(specialEvent.Date.Date < _dateTimeService.Now.Date)
        {
            throw new InvalidOperationException("Cannot purchase tickets for past events");
        }

        var ticket = new Ticket
        {
            VisitorParkInformationId = visitor.VisitorParkInformation.Id,
            Type = nameof(TicketType.SpecialEvent),
            SpecialEventId = eventId,
            Date = specialEvent.Date
        };

        _repository.Add(ticket);
        _ses.IncrementEventCapacity(eventId);
    }
}
