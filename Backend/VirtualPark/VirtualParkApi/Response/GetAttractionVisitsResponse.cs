namespace VirtualParkApi.Response;

public class GetAttractionVisitsResponse
{
    public int AttractionId { get; set; }
    public int VisitsCount { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
}
