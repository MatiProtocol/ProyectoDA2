using Abstraction;
using Domain;
using NonDomainAbstractions;

namespace Services;

public class IncidenceService(IIncidenceRepository incidenceRepository, IAttractionRepository attractionRepository, IDateTimeService dt) : IIncidenceService
{
    private readonly IIncidenceRepository _repository = incidenceRepository;
    private readonly IAttractionRepository _attractionRepository = attractionRepository;
    private readonly IDateTimeService _dateTimeService = dt;

    public Incidence? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public IEnumerable<Incidence> GetAll()
    {
        return _repository.GetAll();
    }

    public void Create(Incidence entity)
    {
        ValidateIncidence(entity);
        var attraction = _attractionRepository.GetById(entity.AttractionId);
        if(attraction == null)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException($"No attraction found for id: {entity.AttractionId}", "Id"));
        }

        entity.AttractionName = attraction.Name;

        _repository.Add(entity);
    }

    public void Update(Incidence entity)
    {
        ValidateIncidence(entity);
        var attraction = _attractionRepository.GetById(entity.AttractionId);
        if(attraction == null)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException($"No attraction found for id: {entity.AttractionId}", "Id"));
        }

        entity.AttractionName = attraction.Name;
        _repository.Update(entity);
    }

    public void Delete(int id)
    {
        var incidence = GetIncidenceOrThrow(id);
        _repository.Remove(incidence);
    }

    public IEnumerable<Incidence> GetActiveIncidences()
    {
        return _repository.GetActiveIncidences();
    }

    public IEnumerable<Incidence> GetByAttractionId(int attractionId)
    {
        return _repository.GetByAttractionId(attractionId);
    }

    public void DeactivateIncidence(int id)
    {
        var incidence = GetIncidenceOrThrow(id);
        incidence.Active = false;
        _repository.Update(incidence);
    }

    public void ActivateIncidence(int id)
    {
        var incidence = GetIncidenceOrThrow(id);
        incidence.Active = true;
        _repository.Update(incidence);
    }

    private void ValidateIncidence(Incidence entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        if(string.IsNullOrWhiteSpace(entity.Description))
        {
            throw new ArgumentException("Incidence description cannot be null or empty", nameof(entity.Description));
        }

        if(entity.AttractionId <= 0)
        {
            throw new ArgumentException("AttractionId must be greater than 0", nameof(entity.AttractionId));
        }

        if(entity.IsMaintenance)
        {
            ValidateMaintenance(entity);
        }
    }

    private void ValidateMaintenance(Incidence entity)
    {
        if(_dateTimeService.Now.Date > entity.StartDate)
        {
            throw new ArgumentException("Maintenance date cannot be in the past", nameof(entity.StartDate));
        }

        if(_dateTimeService.Now.Date == entity.StartDate && _dateTimeService.Now.TimeOfDay > entity.StartTime)
        {
            throw new ArgumentException("Maintenance start time cannot be in the past", nameof(entity.StartTime));
        }

        if(entity.Duration <= 0)
        {
            throw new ArgumentException("Maintenance duration cannot be zero or negative", nameof(entity.Duration));
        }
    }

    private Incidence GetIncidenceOrThrow(int id)
    {
        var incidence = _repository.GetById(id);
        if(incidence == null)
        {
            throw new ArgumentException($"No incidence found for id: {id}", nameof(id));
        }

        return incidence;
    }
}
