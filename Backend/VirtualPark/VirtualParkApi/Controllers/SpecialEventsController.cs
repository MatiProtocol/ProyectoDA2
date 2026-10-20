using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecialEventsController(ISpecialEventService specialEventService) : ControllerBase
{
    private readonly ISpecialEventService _specialEventService = specialEventService;

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpPost]
    public IActionResult Create([FromBody] CreateSpecialEventRequest model)
    {
        _specialEventService.Create(model.ToEntity());
        _specialEventService.AddAttractions(model.Name, model.AttractionsIds);
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpDelete("{id:int}")]
    public IActionResult Remove(int id)
    {
        _specialEventService.DeactivateEvent(id);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var specialEvent = _specialEventService.GetById(id)!;
        return Ok(new GetSpecialEventResponse(specialEvent));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var events = _specialEventService.GetAll();
        var aux = events.Select(e => new GetSpecialEventResponse(e)).AsQueryable();
        return Ok(aux);
    }

    [HttpGet("active")]
    public IActionResult GetActiveEvents()
    {
        var events = _specialEventService.GetActiveEvents();
        return Ok(events.Select(e => new GetSpecialEventResponse(e)).AsQueryable());
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] CreateSpecialEventRequest model)
    {
        _specialEventService.Update(model.ToEntity(id));
        return Ok();
    }
}
