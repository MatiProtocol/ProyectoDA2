namespace Domain;

public class Incidence
{
    private int _id;
    private string _description;
    private bool _active;
    private int _attractionId;
    private string _attractionName;
    private DateTime _startDate;
    private bool _isMaintenance;
    private TimeSpan _startTime;
    private int _duration;
    public Incidence()
    {
        _description = string.Empty;
        _active = false;
        _attractionName = string.Empty;
        _isMaintenance = false;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public int AttractionId
    {
        get => _attractionId;
        set => _attractionId = value;
    }

    public string AttractionName
    {
        get => _attractionName;
        set => _attractionName = value;
    }

    public DateTime StartDate
    {
        get => _startDate;
        set => _startDate = value;
    }

    public bool IsMaintenance
    {
        get => _isMaintenance;
        set => _isMaintenance = value;
    }

    public TimeSpan StartTime
    {
        get => _startTime;
        set => _startTime = value;
    }

    public int Duration
    {
        get => _duration;
        set => _duration = value;
    }
}
