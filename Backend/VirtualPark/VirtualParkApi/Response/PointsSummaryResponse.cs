namespace VirtualParkApi.Response;

public class PointsSummaryResponse
{
    public int VisitorId { get; set; }
    public int TotalPointsEarned { get; set; }
    public int TotalPointsSpent { get; set; }
    public int CurrentBalance { get; set; }
    public int TodayPoints { get; set; }
    public int RedemptionsToday { get; set; }
}
