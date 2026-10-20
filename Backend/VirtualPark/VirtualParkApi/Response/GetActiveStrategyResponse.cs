namespace VirtualParkApi.Response;

public class GetActiveStrategyResponse(string strategyName)
{
    public string StrategyName { get; set; } = strategyName;
}
