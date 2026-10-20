using Domain;

namespace VirtualParkApi.Request;

public class CreateAttractionRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Type { get; set; }
    public required int MinAge { get; set; }
    public required int MaxCapacity { get; set; }
    public required int CurrentCapacity { get; set; }

    public Attraction ToEntity(int id = 0)
    {
        return new Attraction
        {
            Id = id,
            Name = Name,
            Description = Description,
            TypeAttraction = SetTypeAttraction(),
            MinAge = MinAge,
            MaxCapacity = MaxCapacity,
            CurrentCapacity = CurrentCapacity
        };
    }

    private TypeAttraction SetTypeAttraction()
    {
        try
        {
            return Enum.Parse<TypeAttraction>(Type);
        }
        catch(ArgumentException e)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException(e.Message, "Type"));
        }
    }
}
