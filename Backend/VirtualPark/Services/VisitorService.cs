using Abstraction;
using Domain;
using Exceptions;
using NonDomainAbstractions;

namespace Services;

public class VisitorService(
    IVisitorRepository rp,
    IDateTimeService dt) : IVisitorService
{
    private readonly IVisitorRepository _repository = rp;
    private readonly IDateTimeService _dateTimeService = dt;

    public Visitor GetById(int id)
    {
        return GetAux(id);
    }

    public IEnumerable<Visitor> GetAll(string? email)
    {
        if(email == null)
        {
            var allVisitors = _repository.GetAll();
            var visitorsWithInfo = new List<Visitor>();
            foreach(var visitor in allVisitors)
            {
                var visitorWithInfo = GetVisitorWithParkInformation(visitor.Id);
                visitorsWithInfo.Add(visitorWithInfo);
            }

            return visitorsWithInfo.AsEnumerable();
        }

        var response = _repository.GetByEmail(email);
        if(response == null)
        {
            return new List<Visitor>().AsEnumerable();
        }

        var responseWithInfo = _repository.GetVisitorWithParkInfo(response.Id);
        return responseWithInfo != null
            ? new List<Visitor> { responseWithInfo }.AsEnumerable()
            : new List<Visitor>().AsEnumerable();
    }

    public void Create(Visitor entity)
    {
        if(_repository.GetByEmail(entity.Email) != null)
        {
            throw new DbCreatedUniqueOrKeyException("The email already exists");
        }

        _repository.Add(entity);
    }

    public void Update(Visitor entity, string? oldPassword = null)
    {
        if(entity.Id <= 0)
        {
            throw new ArgumentNullException($"No visitor found with id {entity.Id}", new DbKeyDoesNotExistException());
        }

        var changePassword = false;
        if(oldPassword != null)
        {
            var visitor = GetById(entity.Id);
            changePassword = visitor.Password == oldPassword;
            if(!changePassword)
            {
                throw new InvalidOperationException(string.Empty, new ArgumentException("Password does not match with user password", "Password"));
            }
        }

        _repository.Update(entity, changePassword);
    }

    public void Delete(int id)
    {
        var visitor = _repository.GetById(id);
        _repository.Remove(visitor!);
    }

    public Guid GetVisitorNFC(int id)
    {
        return GetVisitorWithParkInformation(id).VisitorParkInformation.NFC;
    }

    public Visitor? GetByEmail(string email)
    {
        var visitor = _repository.GetByEmail(email);
        return visitor;
    }

    public IQueryable<Ticket> GetAvailableTicketsByNfc(Guid nfc)
    {
        return _repository.GetAvailableTicketsByNfc(nfc).Where(t => t.Date.Date == _dateTimeService.Now.Date);
    }

    public int GetVisitsBetween(int attractionId, DateTime startDate, DateTime endDate)
    {
        return _repository.GetVisitsBetween(attractionId, startDate, endDate);
    }

    public Visitor GetVisitorWithParkInformation(int id)
    {
        var visitor = _repository.GetVisitorWithParkInfo(id);

        if(visitor == null)
        {
            throw new ArgumentNullException($"No visitor found with id {id}", new DbKeyDoesNotExistException());
        }

        return visitor;
    }

    public void UpdateVisitorParkInformation(VisitorParkInformation visitorParkInformation)
    {
        _repository.UpdateVisitorParkInformation(visitorParkInformation);
    }

    public IQueryable<Ticket> GetAvailableTicketsById(int id)
    {
        return _repository.GetAvailableTicketsById(id).Where(t => t.Date.Date == _dateTimeService.Now.Date);
    }

    private Visitor GetAux(int id)
    {
        var visitor = _repository.GetById(id);
        if(visitor == null)
        {
            throw new ArgumentNullException($"No visitor found for id: {id}", new DbKeyDoesNotExistException());
        }

        return visitor;
    }
}
