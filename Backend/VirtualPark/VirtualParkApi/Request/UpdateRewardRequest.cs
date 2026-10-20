using Domain;

namespace VirtualParkApi.Request;

public class UpdateRewardRequest
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int PointsCost { get; set; }
    public required int AvailableQuantity { get; set; }
    public string? RequiredMembershipLevel { get; set; }
    public required bool Active { get; set; }

    public Reward ToEntity(int id)
    {
        return new Reward
        {
            Id = id,
            Name = Name,
            Description = Description,
            PointsCost = PointsCost,
            AvailableQuantity = AvailableQuantity,
            RequiredMembershipLevel = RequiredMembershipLevel,
            Active = Active
        };
    }
}
