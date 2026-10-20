namespace VirtualParkApi.Request;

public class GetPointsHistoryRequest
{
    public required int VisitorId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
