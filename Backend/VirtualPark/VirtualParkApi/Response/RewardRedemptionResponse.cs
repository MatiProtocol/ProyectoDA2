using Domain;

namespace VirtualParkApi.Response;

public class RewardRedemptionResponse(RewardRedemption redemption, string rewardName, int remainingPoints)
{
    public int RedemptionId { get; set; } = redemption.Id;
    public string RewardName { get; set; } = rewardName;
    public int PointsSpent { get; set; } = redemption.PointsSpent;
    public DateTime RedemptionDate { get; set; } = redemption.RedemptionDate;
    public string Status { get; set; } = redemption.Status.ToString();
    public int RemainingPoints { get; set; } = remainingPoints;
}
