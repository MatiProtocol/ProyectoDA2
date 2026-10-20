using Domain;

namespace VirtualParkApi.Request;

public class CreateSpecialEventRequest
{
    public required string Name { get; set; }
    public required DateTime Date { get; set; }
    public required string StartTime { get; set; }
    public required string EndTime { get; set; }
    public required int MaxCapacity { get; set; }
    public required decimal AdditionalCost { get; set; }
    public required List<int> AttractionsIds { get; set; }

    public SpecialEvent ToEntity(int id = 0)
    {
        return new SpecialEvent
        {
            Id = id,
            Name = Name,
            Date = Date,
            StartTime = TimeSpan.Parse(StartTime),
            EndTime = TimeSpan.Parse(EndTime),
            MaxCapacity = MaxCapacity,
            AdditionalCost = AdditionalCost,
            CurrentCapacity = 0,
            Active = true
        };
    }
}
