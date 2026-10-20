using Domain;

namespace Tests.DomainTest;

[TestClass]
public class TicketTest
{
    [TestMethod]
    public void Tickets_NotNull()
    {
        var ticket = new Ticket();

        Assert.IsNotNull(ticket);
    }

    [TestMethod]
    public void SetVisitorParkInformationId_GetCorrectly()
    {
        var ticket = new Ticket
        {
            VisitorParkInformationId = 1
        };

        Assert.AreEqual(1, ticket.VisitorParkInformationId);
    }

    [TestMethod]
    public void SetId_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Id = 1
        };

        Assert.AreEqual(1, ticket.Id);
    }

    [TestMethod]
    public void SetType_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Type = nameof(TicketType.General)
        };

        Assert.AreEqual(nameof(TicketType.General), ticket.Type);
    }

    [TestMethod]
    public void SetType_EspecialEvent_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Type = nameof(TicketType.SpecialEvent)
        };

        Assert.AreEqual(nameof(TicketType.SpecialEvent), ticket.Type);
    }

    [TestMethod]
    public void SetDate_GetCorrectly()
    {
        var date = DateTime.Now;
        var ticket = new Ticket
        {
            Date = date
        };

        Assert.AreEqual(ticket.Date, date);
    }

    [TestMethod]
    public void SetQR_GetCorrectly()
    {
        var ticket = new Ticket
        {
            QR = Guid.Empty
        };

        Assert.AreEqual(Guid.Empty, ticket.QR);
    }

    [TestMethod]
    public void SetConsumed_GetCorrectly()
    {
        var ticket = new Ticket
        {
            Consumed = true
        };

        Assert.IsTrue(ticket.Consumed);
    }

    [TestMethod]
    public void SetSpecialEvent_GetCorrectly()
    {
        var ticket = new Ticket
        {
            SpecialEventId = 1
        };

        Assert.AreEqual(1, ticket.SpecialEventId);
    }
}
