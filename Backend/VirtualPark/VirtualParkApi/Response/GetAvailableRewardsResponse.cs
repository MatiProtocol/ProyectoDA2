using Domain;

namespace VirtualParkApi.Response;

public class GetAvailableRewardsResponse(List<Reward> rewards, int visitorPoints, string membership)
{
    public List<RewardItemResponse> Rewards { get; set; } = rewards.Select(r => new RewardItemResponse(r)).ToList();
    public int VisitorPoints { get; set; } = visitorPoints;
    public string VisitorMembership { get; set; } = membership;
}

public class RewardItemResponse(Reward reward)
{
    public int Id { get; set; } = reward.Id;
    public string Name { get; set; } = reward.Name;
    public string Description { get; set; } = reward.Description;
    public int PointsCost { get; set; } = reward.PointsCost;
    public int AvailableQuantity { get; set; } = reward.AvailableQuantity;
    public bool CanRedeem { get; set; } = reward.IsAvailable();
}
