using NonDomainAbstractions;

namespace Domain;

public class PunctuationContext : IPunctuationContext
{
    private int _visitorParkInformationId;
    private VisitedAttraction _actualAttraction = null!;
    private List<IContextVisitedAttraction> _previousAccesses = [];

    public int VisitorParkInformationId
    {
        get => _visitorParkInformationId;
        set => _visitorParkInformationId = value;
    }

    public VisitedAttraction ActualAttraction
    {
        get => _actualAttraction;
        set => _actualAttraction = value;
    }

    public string ActualAttractionType
    {
        get => _actualAttraction.AttractionType.ToString();
        set => _actualAttraction.AttractionType = Enum.Parse<TypeAttraction>(value);
    }

    public DateTime ActualAttractionAccessTime
    {
        get => _actualAttraction.VisitDate;
        set => _actualAttraction.VisitDate = value;
    }

    public List<IContextVisitedAttraction> PreviousAccesses
    {
        get => _previousAccesses;
        set => _previousAccesses = value;
    }

    public bool ActualAttractionIsSpecial
    {
        get => _actualAttraction.IsSpecialEvent;
        set => _actualAttraction.IsSpecialEvent = value;
    }
}
