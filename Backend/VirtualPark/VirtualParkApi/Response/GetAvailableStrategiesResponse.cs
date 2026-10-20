namespace VirtualParkApi.Response;

public class GetAvailableStrategiesResponse(List<string> strategies)
{
    public List<string> Strategies { get; set; } = strategies;
}
