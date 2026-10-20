namespace Domain;

public class SpecialEvent
{
    private int _id;
    private string _name;
    private DateTime _date;
    private TimeSpan _startTime;
    private TimeSpan _endTime;
    private int _maxCapacity;
    private decimal _additionalCost;
    private List<Attraction> _includedAttractions;
    private bool _active;
    private int _currentCapacity;
    public SpecialEvent()
    {
        _name = string.Empty;
        _includedAttractions = [];
        _active = true;
        _currentCapacity = 0;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public DateTime Date
    {
        get => _date;
        set => _date = value;
    }

    public TimeSpan StartTime
    {
        get => _startTime;
        set => _startTime = value;
    }

    public TimeSpan EndTime
    {
        get => _endTime;
        set => _endTime = value;
    }

    public int MaxCapacity
    {
        get => _maxCapacity;
        set => _maxCapacity = value;
    }

    public int CurrentCapacity
    {
        get => _currentCapacity;
        set => _currentCapacity = value;
    }

    public decimal AdditionalCost
    {
        get => _additionalCost;
        set => _additionalCost = value;
    }

    public List<Attraction> IncludedAttractions
    {
        get => _includedAttractions;
        set => _includedAttractions = value;
    }

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public bool IsCapacityAvailable()
    {
        return _currentCapacity < _maxCapacity;
    }

    public bool IncrementCapacity()
    {
        if(IsCapacityAvailable())
        {
            _currentCapacity++;
            return true;
        }

        return false;
    }

    public bool DecrementCapacity()
    {
        if(_currentCapacity > 0)
        {
            _currentCapacity--;
            return true;
        }

        return false;
    }

    public bool IsEventActive(DateTime currentDateTime)
    {
        if(!_active)
        {
            return false;
        }

        var eventStartDateTime = _date.Date + _startTime;
        var eventEndDateTime = _date.Date + _endTime;
        return currentDateTime >= eventStartDateTime && currentDateTime <= eventEndDateTime;
    }

    public bool ContainsAttraction(int attractionId)
    {
        return _includedAttractions.Any(a => a.Id == attractionId);
    }
}
