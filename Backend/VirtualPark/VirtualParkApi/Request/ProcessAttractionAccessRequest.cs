namespace VirtualParkApi.Request;
public class ProcessAttractionAccessRequest
{
    public int? VisitorId { get; set; }
    public required int AttractionId { get; set; }
    public string? VisitorNFC { get; set; }
}
