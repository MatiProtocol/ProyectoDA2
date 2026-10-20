using Domain;

namespace VirtualParkApi.Response;

public class GetAttractionResponse(Attraction attraction)
{
    public int Id { get; set; } = attraction.Id;
    public string? Name { get; set; } = attraction.Name;
    public string Type { get; set; } = attraction.TypeAttraction.ToString();
    public int MinAge { get; set; } = attraction.MinAge;
    public int MaxCapacity { get; set; } = attraction.MaxCapacity;
    public int CurrentCapacity { get; set; } = attraction.CurrentCapacity;
    public string? Description { get; set; } = attraction.Description;
}
