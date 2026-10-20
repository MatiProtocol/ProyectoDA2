namespace VirtualParkApi.Response;

public class GetVisitorPointsResponse(int visitorId, int totalPoints)
{
    public int VisitorId { get; set; } = visitorId;
    public int TotalPoints { get; set; } = totalPoints;
}
