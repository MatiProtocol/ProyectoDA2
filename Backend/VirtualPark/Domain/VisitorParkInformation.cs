namespace Domain;

public class VisitorParkInformation
{
    private int _id;
    private string _visitorMembership;
    private int _points;
    private int _redeemPointsToday;
    private int _todayPoints;
    private List<Ticket> _tickets;
    private Guid _nfc;
    private List<VisitedAttraction> _visitedAttractions;
    private List<RewardRedemption> _rewardRedemptions;
    private List<PointsHistory> _pointsHistory;

    public VisitorParkInformation()
    {
        _tickets = [];
        _visitorMembership = nameof(Membership.Standard);
        _visitedAttractions = [];
        _rewardRedemptions = [];
        _pointsHistory = [];
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string VisitorMembership
    {
        get => _visitorMembership;
        set => _visitorMembership = value;
    }

    public int Points
    {
        get => _points;
        set => _points = value;
    }

    public List<Ticket> Tickets
    {
        get => _tickets;
        set => _tickets = value;
    }

    public List<VisitedAttraction> VisitedAttractions
    {
        get => _visitedAttractions;
        set => _visitedAttractions = value;
    }

    public Guid NFC
    {
        get => _nfc;
        set => _nfc = value;
    }

    public int TodayPoints
    {
        get => _todayPoints;
        set => _todayPoints = value;
    }

    public int RedeemPointsToday
    {
        get => _redeemPointsToday;
        set => _redeemPointsToday = value;
    }

    public List<RewardRedemption> RewardRedemptions
    {
        get => _rewardRedemptions;
        set => _rewardRedemptions = value;
    }

    public List<PointsHistory> PointsHistory
    {
        get => _pointsHistory;
        set => _pointsHistory = value;
    }
}
