namespace Domain;

/// <summary>
/// Define los tipos de Atracciones que se pueden crear.
/// </summary>
public enum TypeAttraction
{
    /// <summary>
    /// Representa al RollerCoaster.
    /// </summary>
    RollerCoaster,

    /// <summary>
    /// Representa al Simulator.
    /// </summary>
    Simulator,

    /// <summary>
    /// Representa al Show.
    /// </summary>
    Show,

    /// <summary>
    /// Representa al InteractiveZone.
    /// </summary>
    InteractiveZone
}

public class Attraction
{
    private int _id;
    private string _name;
    private TypeAttraction _typeAttraction;
    private int _minAge;
    private int _maxCapacity;
    private int _currentCapacity;
    private string _description;
    private List<Incidence> _incidences;

    public Attraction()
    {
        _name = string.Empty;
        _description = string.Empty;
        _incidences = [];
        _currentCapacity = 0;
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public TypeAttraction TypeAttraction
    {
        get => _typeAttraction;
        set => _typeAttraction = value;
    }

    public int MinAge
    {
        get => _minAge;
        set => _minAge = value;
    }

    public int MaxCapacity
    {
        get => _maxCapacity;
        set => _maxCapacity = value;
    }

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public int CurrentCapacity
    {
        get => _currentCapacity;
        set => _currentCapacity = value;
    }

    public List<Incidence> Incidences
    {
        get => _incidences;
        set => _incidences = value;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }

    public bool HasActiveIncidence()
    {
        return _incidences.Any(i => i.Active);
    }

    public bool IsCapacityAvailable()
    {
        return _currentCapacity < _maxCapacity;
    }

    public int GetAvailableCapacity()
    {
        return _maxCapacity - _currentCapacity;
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
}
