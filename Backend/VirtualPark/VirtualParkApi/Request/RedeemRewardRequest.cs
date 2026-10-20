namespace VirtualParkApi.Request;

public class RedeemRewardRequest
{
    public required int VisitorId { get; set; }
    public required int RewardId { get; set; }
}
