namespace VirtualParkApi.Request;

public class AddPointsRequest
{
    public required int VisitorId { get; set; }
    public required int Points { get; set; }
}
