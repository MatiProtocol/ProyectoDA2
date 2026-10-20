namespace Domain;

public class Visitor : User
{
    private DateTime _birthdate;
    private VisitorParkInformation _visitorParkInformation;

    public Visitor(VisitorConstructHelper helper)
        : base(helper.Name, helper.Password, helper.Email)
    {
        _birthdate = helper.Birthdate;
        _visitorParkInformation = new VisitorParkInformation();
    }

    public Visitor()
    {
        _visitorParkInformation = new VisitorParkInformation();
    }

    public struct VisitorConstructHelper
    {
        public string Name;
        public string Email;
        public string Password;
        public DateTime Birthdate;
    }

    public DateTime BirthDate
    {
        get => _birthdate;
        set => _birthdate = value;
    }

    public VisitorParkInformation VisitorParkInformation
    {
        get => _visitorParkInformation;
        set => _visitorParkInformation = value;
    }

    public Guid GetIdentifierNFC()
    {
        return _visitorParkInformation.NFC;
    }
}
