namespace VirtualParkApi.Response;

public class GetCapacityResponse
{
    public required int CurrentCapacity { get; set; }
    public int RemainingCapacity { get; set; }
}
