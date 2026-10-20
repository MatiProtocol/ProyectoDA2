using Domain;

namespace VirtualParkApi.Response;

public class GetSpecialEventResponse(SpecialEvent specialEvent)
{
    public int Id { get; set; } = specialEvent.Id;
    public string Name { get; set; } = specialEvent.Name;
    public string Date { get; set; } = specialEvent.Date.ToString("yyyy-MM-dd");
    public string StartTime { get; set; } = specialEvent.StartTime.ToString(@"hh\:mm");
    public string EndTime { get; set; } = specialEvent.EndTime.ToString(@"hh\:mm");
    public int MaxCapacity { get; set; } = specialEvent.MaxCapacity;
    public int CurrentCapacity { get; set; } = specialEvent.CurrentCapacity;
    public decimal AdditionalCost { get; set; } = specialEvent.AdditionalCost;
    public List<int> IncludedAttractionIds { get; set; } = specialEvent.IncludedAttractions.Select(a => a.Id).ToList();
    public bool Active { get; set; } = specialEvent.Active;
}
