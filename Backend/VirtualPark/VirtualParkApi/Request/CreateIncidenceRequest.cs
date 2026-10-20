using Domain;

namespace VirtualParkApi.Request;

public class CreateIncidenceRequest
{
    public required string Description { get; set; }
    public required int AttractionId { get; set; }
    public bool IsMaintenance { get; set; }
    public DateTime? StartDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public int? Duration { get; set; }

    public Incidence ToEntity(int id = 0)
    {
        if(IsMaintenance)
        {
            return new Incidence
            {
                Id = id,
                Description = Description,
                AttractionId = AttractionId,
                Active = true,
                IsMaintenance = IsMaintenance,
                StartDate = StartDate!.Value,
                StartTime = StartTime!.Value,
                Duration = Duration!.Value
            };
        }

        return new Incidence
        {
            Id = id,
            Description = Description,
            AttractionId = AttractionId,
            Active = true,
            IsMaintenance = IsMaintenance
        };
    }
}
