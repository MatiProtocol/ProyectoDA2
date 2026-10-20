using Domain;

namespace VirtualParkApi.Response;

public class GetIncidenceResponse(Incidence incidence)
{
    public int Id { get; set; } = incidence.Id;
    public string Description { get; set; } = incidence.Description;
    public int AttractionId { get; set; } = incidence.AttractionId;
    public string AttractionName { get; set; } = incidence.AttractionName;
    public bool IsMaintenance { get; set; } = incidence.IsMaintenance;
    public DateTime StartDate { get; set; } = incidence.StartDate;
    public TimeSpan StartTime { get; set; } = incidence.StartTime;
    public int Duration { get; set; } = incidence.Duration;
    public bool Active { get; set; } = incidence.Active;
}
