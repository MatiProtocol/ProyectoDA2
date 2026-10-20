using Domain;

namespace VirtualParkApi.Response;

public class GetVisitorRedemptionsResponse(List<RewardRedemption> redemptions)
{
    public List<RedemptionHistoryItem> Redemptions { get; set; } = redemptions.Select(r => new RedemptionHistoryItem(r)).ToList();
    public int TotalPointsRedeemed { get; set; } = redemptions.Sum(r => r.PointsSpent);
}

public class RedemptionHistoryItem(RewardRedemption redemption)
{
    public int Id { get; set; } = redemption.Id;
    public int RewardId { get; set; } = redemption.RewardId;
    public DateTime RedemptionDate { get; set; } = redemption.RedemptionDate;
    public int PointsSpent { get; set; } = redemption.PointsSpent;
    public string Status { get; set; } = redemption.Status.ToString();
}
