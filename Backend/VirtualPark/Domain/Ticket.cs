namespace Domain;

public class Ticket
{
    private int _id;
    private int _visitorParkInformationId;
    private string _type;
    private DateTime _date;
    private Guid _qr;
    private bool _consumed;
    private int? _specialEventId;

    public Ticket()
    {
        _date = DateTime.Now;
        _type = nameof(TicketType.General);
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

    public string Type
    {
        get => _type;
        set => _type = value;
    }

    public DateTime Date
    {
        get => _date;
        set => _date = value;
    }

    public Guid QR
    {
        get => _qr;
        set => _qr = value;
    }

    public bool Consumed
    {
        get => _consumed;
        set => _consumed = value;
    }

    public int? SpecialEventId
    {
        get => _specialEventId;
        set => _specialEventId = value;
    }
}
