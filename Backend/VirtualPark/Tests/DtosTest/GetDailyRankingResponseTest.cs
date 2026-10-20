using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetDailyRankingResponseTest
{
    private Dictionary<string, int> _ranking = null!;
    private DateTime _rankingDate;

    [TestInitialize]
    public void Initialize()
    {
        _ranking = new Dictionary<string, int>
        {
            { "User1 - user1@email.com", 100 },
            { "User2 - user2@email.com", 80 }
        };
        _rankingDate = new DateTime(2025, 10, 2);
    }

    [TestMethod]
    public void GetDailyRankingResponse_WithValidData_CreatesInstance()
    {
        var response = new GetDailyRankingResponse(_ranking, _rankingDate);

        Assert.IsNotNull(response);
    }

    [TestMethod]
    public void GetDailyRankingResponse_SetsRankingDate()
    {
        var response = new GetDailyRankingResponse(_ranking, _rankingDate);

        Assert.AreEqual(_rankingDate, response.RankingDate);
    }

    [TestMethod]
    public void GetDailyRankingResponse_CreatesRankingList()
    {
        var response = new GetDailyRankingResponse(_ranking, _rankingDate);

        Assert.AreEqual(2, response.Ranking.Count);
    }

    [TestMethod]
    public void GetDailyRankingResponse_AssignsSequentialPositions()
    {
        var response = new GetDailyRankingResponse(_ranking, _rankingDate);

        Assert.AreEqual(1, response.Ranking[0].Position);
    }

    [TestMethod]
    public void GetDailyRankingResponse_SecondEntryHasPositionTwo()
    {
        var response = new GetDailyRankingResponse(_ranking, _rankingDate);

        Assert.AreEqual(2, response.Ranking[1].Position);
    }

    [TestMethod]
    public void GetDailyRankingResponse_WithEmptyRanking_SetsEmptyList()
    {
        var emptyRanking = new Dictionary<string, int>();
        var response = new GetDailyRankingResponse(emptyRanking, _rankingDate);

        Assert.AreEqual(0, response.Ranking.Count);
    }

    [TestMethod]
    public void GetDailyRankingResponse_WithThreeEntries_CreatesThreeRankingEntries()
    {
        var ranking = new Dictionary<string, int>
        {
            { "User1", 100 },
            { "User2", 80 },
            { "User3", 60 }
        };

        var response = new GetDailyRankingResponse(ranking, _rankingDate);

        Assert.AreEqual(3, response.Ranking.Count);
    }

    [TestMethod]
    public void GetDailyRankingResponse_ThirdEntryHasPositionThree()
    {
        var ranking = new Dictionary<string, int>
        {
            { "User1", 100 },
            { "User2", 80 },
            { "User3", 60 }
        };

        var response = new GetDailyRankingResponse(ranking, _rankingDate);

        Assert.AreEqual(3, response.Ranking[2].Position);
    }

    [TestMethod]
    public void GetDailyRankingResponse_FirstEntryContainsVisitorName()
    {
        var response = new GetDailyRankingResponse(_ranking, _rankingDate);

        Assert.AreEqual("User1 - user1@email.com", response.Ranking[0].VisitorName);
    }

    [TestMethod]
    public void GetDailyRankingResponse_FirstEntryContainsPoints()
    {
        var response = new GetDailyRankingResponse(_ranking, _rankingDate);

        Assert.AreEqual(100, response.Ranking[0].Points);
    }
}
