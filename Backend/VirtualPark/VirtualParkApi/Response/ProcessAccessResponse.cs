namespace VirtualParkApi.Response;

public class ProcessAccessResponse(int? visitorId, int pointsAwarded, int totalPoints)
{
    public int? VisitorId { get; set; } = visitorId;
    public int PointsAwarded { get; set; } = pointsAwarded;
    public int TotalPoints { get; set; } = totalPoints;
}
