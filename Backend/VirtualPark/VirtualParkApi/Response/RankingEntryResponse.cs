namespace VirtualParkApi.Response;
public class RankingEntryResponse(string visitorName, int points, int position)
{
    public string VisitorName { get; set; } = visitorName;
    public int Points { get; set; } = points;
    public int Position { get; set; } = position;
}
