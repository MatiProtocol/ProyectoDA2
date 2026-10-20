namespace Domain;

/// <summary>
/// Represent redemption status.
/// </summary>
public enum RedemptionStatus
{
    /// <summary>
    /// Pending status.
    /// </summary>
    Pending,

    /// <summary>
    /// Completed status.
    /// </summary>
    Completed,

    /// <summary>
    /// Cancelled status.
    /// </summary>
    Cancelled
}

public class RewardRedemption
{
    private int _id;
    private DateTime _redemptionDate;
    private RedemptionStatus _status;
    private int _visitorParkInformationId;
    private int _rewardId;
    private int _pointsSpent;

    public RewardRedemption()
    {
        _redemptionDate = DateTime.Now;
        _status = RedemptionStatus.Pending;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public int VisitorParkInformationId
    {
        get => _visitorParkInformationId;
        set => _visitorParkInformationId = value;
    }

    public int RewardId
    {
        get => _rewardId;
        set => _rewardId = value;
    }

    public DateTime RedemptionDate
    {
        get => _redemptionDate;
        set => _redemptionDate = value;
    }

    public int PointsSpent
    {
        get => _pointsSpent;
        set => _pointsSpent = value;
    }

    public RedemptionStatus Status
    {
        get => _status;
        set => _status = value;
    }

    public Reward? Reward { get; set; }
    public VisitorParkInformation? VisitorParkInformation { get; set; }
}
