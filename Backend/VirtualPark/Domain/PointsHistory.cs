namespace Domain;

public class PointsHistory
{
    private DateTime _transactionDate;
    private string _origin;
    private string _description;
    private string _activeStrategy;
    private int _id;
    private int _visitorParkInformationId;
    private int _points;
    private int? _relatedEntityId;

    public PointsHistory()
    {
        _transactionDate = DateTime.Now;
        _origin = string.Empty;
        _description = string.Empty;
        _activeStrategy = string.Empty;
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

    public DateTime TransactionDate
    {
        get => _transactionDate;
        set => _transactionDate = value;
    }

    public int Points
    {
        get => _points;
        set => _points = value;
    }

    public string Origin
    {
        get => _origin;
        set => _origin = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public string ActiveStrategy
    {
        get => _activeStrategy;
        set => _activeStrategy = value;
    }

    public int? RelatedEntityId
    {
        get => _relatedEntityId;
        set => _relatedEntityId = value;
    }
}
