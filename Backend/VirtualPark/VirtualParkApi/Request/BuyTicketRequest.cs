namespace VirtualParkApi.Request;

public class BuyTicketRequest
{
    public required int VisitorId { get; set; }
    public int EventId { get; set; }
}
