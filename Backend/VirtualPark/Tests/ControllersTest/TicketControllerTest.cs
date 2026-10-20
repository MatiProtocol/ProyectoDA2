using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;

[TestClass]
public class TicketControllerTest
{
    private ITicketController _ticketController = null!;
    private Mock<ITicketService> _ticketService = null!;

    [TestInitialize]
    public void Initialize()
    {
        _ticketService = new Mock<ITicketService>(MockBehavior.Strict);
        _ticketController = new TicketController(_ticketService.Object);
    }

    [TestMethod]
    public void TicketController_IsNotNull()
    {
        Assert.IsNotNull(_ticketController);
    }

    [TestMethod]
    public void Remove_Ok()
    {
        _ticketService.Setup(ts => ts.Delete(1));

        Assert.IsInstanceOfType<OkResult>(_ticketController.Remove(1));
    }

    [TestMethod]
    public void Create_Ok()
    {
        _ticketService.Setup(ts => ts.BuyTicket(1, 0));

        var request = new BuyTicketRequest
        {
            VisitorId = 1
        };

        _ticketController.Create(request);
        Assert.IsInstanceOfType<OkResult>(_ticketController.Create(request));
    }

    [TestMethod]
    public void GetAll_Ok()
    {
        var ticket = new Ticket
        {
            Consumed = false,
            Date = DateTime.Now,
            Id = 1,
            QR = Guid.Empty,
            SpecialEventId = 1,
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        var tickets = new List<Ticket> { ticket }.AsEnumerable();
        _ticketService.Setup(ts => ts.GetAll()).Returns(tickets);

        var response = _ticketController.GetAll() as OkObjectResult;

        var result = response!.Value as IEnumerable<GetTicketResponse>;
        Assert.AreEqual(1, result!.Count());
    }

    [TestMethod]
    public void GetById_Ok()
    {
        var ticket = new Ticket
        {
            Consumed = false,
            Date = DateTime.Now,
            Id = 1,
            QR = Guid.Empty,
            SpecialEventId = 1,
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        _ticketService.Setup(ts => ts.GetById(1)).Returns(ticket);

        var response = _ticketController.GetById(1) as OkObjectResult;

        var result = response!.Value as GetTicketResponse;
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetTicketByQr_Ok()
    {
        var ticket = new Ticket
        {
            Consumed = false,
            Date = DateTime.Now,
            Id = 1,
            QR = Guid.Empty,
            SpecialEventId = 1,
            Type = nameof(TicketType.General),
            VisitorParkInformationId = 1
        };

        _ticketService.Setup(ts => ts.GetTicketByQr(ticket.QR)).Returns(ticket);

        var response = _ticketController.GetTicketByQr(ticket.QR) as OkObjectResult;

        var result = response!.Value as GetTicketResponse;
        Assert.IsNotNull(result);
    }
}
