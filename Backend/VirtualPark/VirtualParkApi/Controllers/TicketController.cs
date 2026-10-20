using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketController(ITicketService ts) : Controller, ITicketController
{
    private readonly ITicketService _ticketService = ts;

    [Authorize]
    [HttpPost]
    public IActionResult Create(BuyTicketRequest model)
    {
        _ticketService.BuyTicket(model.VisitorId, model.EventId);
        return Ok();
    }

    [Authorize]
    [HttpPost("{id:int}")]
    public IActionResult Remove(int id)
    {
        _ticketService.Delete(id);
        return Ok();
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        return Ok(new GetTicketResponse(_ticketService.GetById(id)!));
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_ticketService.GetAll().Select(t => new GetTicketResponse(t)));
    }

    [Authorize]
    [OwnAccountOrRoleFilter(["ParkAdmin,", "Operator,"])]
    [HttpGet("qr")]
    public IActionResult GetTicketByQr([FromQuery] Guid qr)
    {
        return Ok(new GetTicketResponse(_ticketService.GetTicketByQr(qr)!));
    }
}
