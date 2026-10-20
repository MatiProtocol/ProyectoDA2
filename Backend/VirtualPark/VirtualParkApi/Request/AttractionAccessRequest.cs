namespace VirtualParkApi.Request;

public class AttractionAccessRequest
{
    public Guid? TicketQR { get; set; }
    public Guid? VisitorNFC { get; set; }
}
