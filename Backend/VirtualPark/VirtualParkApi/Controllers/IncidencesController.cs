using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class IncidencesController(IIncidenceService incidenceService) : ControllerBase, IIncidenceController
{
    private readonly IIncidenceService _incidenceService = incidenceService;

    [Authorize]
    [RolePermissionFilter(["Operator,", "ParkAdmin,"], false)]
    [HttpPost]
    public IActionResult Create([FromBody] CreateIncidenceRequest model)
    {
        _incidenceService.Create(model.ToEntity());
        return Created();
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var incidence = _incidenceService.GetById(id);
        if(incidence == null)
        {
            return NotFound();
        }

        return Ok(new GetIncidenceResponse(incidence));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var incidences = _incidenceService.GetAll();
        return Ok(incidences.Select(i => new GetIncidenceResponse(i)).AsQueryable());
    }

    [Authorize]
    [RolePermissionFilter(["Operator,", "ParkAdmin,"], false)]
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] CreateIncidenceRequest model)
    {
        _incidenceService.Update(model.ToEntity(id));
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpDelete("{id:int}")]
    public IActionResult Remove(int id)
    {
        _incidenceService.Delete(id);
        return Ok();
    }

    [HttpGet("active")]
    public IActionResult GetActiveIncidences()
    {
        var activeIncidences = _incidenceService.GetActiveIncidences();
        return Ok(activeIncidences.Select(i => new GetIncidenceResponse(i)).AsQueryable());
    }

    [HttpGet("attraction/{attractionId:int}")]
    public IActionResult GetByAttractionId(int attractionId)
    {
        var incidences = _incidenceService.GetByAttractionId(attractionId);
        return Ok(incidences.Select(i => new GetIncidenceResponse(i)).AsQueryable());
    }

    [Authorize]
    [RolePermissionFilter(["Operator,", "ParkAdmin,"])]
    [HttpPatch("{id:int}/deactivate")]
    public IActionResult DeactivateIncidence(int id)
    {
        _incidenceService.DeactivateIncidence(id);
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["Operator,", "ParkAdmin,"], false)]
    [HttpPatch("{id:int}/activate")]
    public IActionResult ActivateIncidence(int id)
    {
        _incidenceService.ActivateIncidence(id);
        return Ok();
    }
}
