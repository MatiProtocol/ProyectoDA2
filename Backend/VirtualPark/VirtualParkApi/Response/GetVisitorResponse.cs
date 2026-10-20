using System.Globalization;
using Domain;

namespace VirtualParkApi.Response;

public class GetVisitorResponse(Visitor visitor)
{
    public int Id { get; set; } = visitor.Id;
    public string Name { get; set; } = visitor.Name;
    public string Email { get; set; } = visitor.Email.ToString();
    public string BirthDate { get; set; } = visitor.BirthDate.ToString(CultureInfo.InvariantCulture);
    public string Membership { get; set; } = visitor.VisitorParkInformation.VisitorMembership;
    public int Points { get; set; } = visitor.VisitorParkInformation.Points;
    public int TodayPoints { get; set; } = visitor.VisitorParkInformation.TodayPoints;
}
