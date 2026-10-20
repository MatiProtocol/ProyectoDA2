using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VisitorsController(IVisitorService visitorService) : ControllerBase, IVisitorController
{
    private readonly IVisitorService _visitorService = visitorService;

    [HttpPost]
    public IActionResult Create([FromBody] CreateVisitorRequest model)
    {
        _visitorService.Create(model.ToEntity());
        return Ok();
    }

    [Authorize]
    [OwnAccountOrRoleFilter(["ParkAdmin,"])]
    [HttpDelete("{id:int}")]
    public IActionResult Remove(int id)
    {
        _visitorService.Delete(id);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        return Ok(new GetVisitorResponse(_visitorService.GetById(id)!));
    }

    [HttpGet]
    public IActionResult GetAll([FromQuery] string? email = null)
    {
        var visitors = _visitorService.GetAll(email);

        return Ok(visitors.Select(visitor => new GetVisitorResponse(visitor)));
    }

    [Authorize]
    [OwnDataAccountFilter]
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, CreateVisitorRequest model)
    {
        _visitorService.Update(model.ToEntity(id), model.OldPassword);
        return Ok();
    }

    [Authorize]
    [OwnAccountOrRoleFilter(["Operator,"])]
    [HttpGet("{id:int}/NFC")]
    public IActionResult GetVisitorNFC(int id)
    {
        var qr = _visitorService.GetVisitorNFC(id);
        return Ok(qr);
    }

    [Authorize]
    [OwnAccountOrRoleFilter(["Operator,"])]
    [HttpGet("{id:int}/Tickets")]
    public IActionResult GetAvailableTicketsById(int id)
    {
        var result = _visitorService.GetAvailableTicketsById(id);
        return Ok(result.Select(ticket => new GetTicketResponse(ticket)));
    }
}
