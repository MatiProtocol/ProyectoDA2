using Domain;

namespace VirtualParkApi.Response;

public class GetTicketResponse(Ticket ticket)
{
    public int Id { get; set; } = ticket.Id;
    public Guid QR { get; set; } = ticket.QR;
    public int VisitorParkInformationId { get; set; } = ticket.VisitorParkInformationId;
    public bool Consumed { get; set; } = ticket.Consumed;
    public string TicketType { get; set; } = ticket.Type;
    public DateTime Date { get; set; } = ticket.Date;
    public int SpecialEventId { get; set; } = ticket.SpecialEventId ??= 0;
}
