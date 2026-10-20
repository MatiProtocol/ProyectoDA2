namespace Domain;

public class Reward
{
    private int _id;
    private string _name;
    private string _description;
    private int _pointsCost;
    private bool _active;
    private int _availableQuantity;
    private string? _requiredMembershipLevel;

    public Reward()
    {
        _name = string.Empty;
        _description = string.Empty;
        _active = true;
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

    public string Description
    {
        get => _description;
        set => _description = value;
    }

    public int PointsCost
    {
        get => _pointsCost;
        set => _pointsCost = value;
    }

    public int AvailableQuantity
    {
        get => _availableQuantity;
        set => _availableQuantity = value;
    }

    public string? RequiredMembershipLevel
    {
        get => _requiredMembershipLevel;
        set => _requiredMembershipLevel = value;
    }

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    public bool IsAvailable()
    {
        return _active && _availableQuantity > 0;
    }

    public bool CanBeRedeemedByMembership(string visitorMembership)
    {
        if(string.IsNullOrEmpty(_requiredMembershipLevel))
        {
            return true;
        }

        return _requiredMembershipLevel.Equals(visitorMembership, StringComparison.OrdinalIgnoreCase);
    }

    public bool DecrementStock()
    {
        if(_availableQuantity <= 0)
        {
            return false;
        }

        _availableQuantity--;
        return true;
    }
}
