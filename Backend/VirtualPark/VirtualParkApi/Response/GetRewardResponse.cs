using Domain;

namespace VirtualParkApi.Response;

public class GetRewardResponse(Reward reward)
{
    public int Id { get; set; } = reward.Id;
    public string Name { get; set; } = reward.Name;
    public string Description { get; set; } = reward.Description;
    public int PointsCost { get; set; } = reward.PointsCost;
    public int AvailableQuantity { get; set; } = reward.AvailableQuantity;
    public string? RequiredMembershipLevel { get; set; } = reward.RequiredMembershipLevel;
    public bool Active { get; set; } = reward.Active;
    public bool IsAvailable { get; set; } = reward.IsAvailable();
}
