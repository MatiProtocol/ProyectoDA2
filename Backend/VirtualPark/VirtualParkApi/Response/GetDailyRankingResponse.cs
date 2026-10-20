namespace VirtualParkApi.Response;

public class GetDailyRankingResponse
{
    public List<RankingEntryResponse> Ranking { get; set; }
    public DateTime RankingDate { get; set; }

    public GetDailyRankingResponse(Dictionary<string, int> ranking, DateTime rankingDate)
    {
        RankingDate = rankingDate;
        Ranking = [];
        var position = 1;
        foreach(var entry in ranking)
        {
            Ranking.Add(new RankingEntryResponse(entry.Key, entry.Value, position));
            position++;
        }
    }
}
