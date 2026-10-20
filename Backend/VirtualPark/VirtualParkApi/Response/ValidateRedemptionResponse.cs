namespace VirtualParkApi.Response;

public class ValidateRedemptionResponse(bool canRedeem, int visitorPoints, int rewardCost, string? reason = null)
{
    public bool CanRedeem { get; set; } = canRedeem;
    public string? ReasonIfCannot { get; set; } = reason;
    public int VisitorPoints { get; set; } = visitorPoints;
    public int RewardCost { get; set; } = rewardCost;
}
