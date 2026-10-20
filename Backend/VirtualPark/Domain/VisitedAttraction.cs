using NonDomainAbstractions;

namespace Domain;

public class VisitedAttraction : IContextVisitedAttraction
{
    private int _visitorParkInformationId;
    private int _attractionId;
    private DateTime _visitDate;
    private TypeAttraction _attractionType;
    private bool _isSpecialEvent;

    public VisitedAttraction()
    {
    }

    public VisitedAttraction(int visitorParkInformationId, int attractionId, DateTime visitDate)
    {
        _visitorParkInformationId = visitorParkInformationId;
        _attractionId = attractionId;
        _visitDate = visitDate;
    }

    public int VisitorParkInformationId
    {
        get => _visitorParkInformationId;
        set => _visitorParkInformationId = value;
    }

    public int AttractionId
    {
        get => _attractionId;
        set => _attractionId = value;
    }

    public DateTime VisitDate
    {
        get => _visitDate;
        set => _visitDate = value;
    }

    public bool IsSpecialEvent
    {
        get => _isSpecialEvent;
        set => _isSpecialEvent = value;
    }

    public TypeAttraction AttractionType
    {
        get => _attractionType;
        set => _attractionType = value;
    }

    public DateTime GetVisitDate()
    {
        return VisitDate;
    }

    public string GetAttractionType()
    {
        return AttractionType.ToString();
    }
}
