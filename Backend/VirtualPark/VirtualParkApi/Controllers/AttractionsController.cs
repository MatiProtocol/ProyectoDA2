using System.Globalization;
using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AttractionsController(IAttractionService attractionService, IIncidenceService incidenceService)
    : Controller, IAttractionController
{
    private readonly IAttractionService _attractionService = attractionService;
    private readonly IIncidenceService _incidenceService = incidenceService;

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpPost]
    public IActionResult Create([FromBody] CreateAttractionRequest model)
    {
        _attractionService.Create(model.ToEntity());
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpDelete("{id:int}")]
    public IActionResult Remove(int id)
    {
        _attractionService.Delete(id);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var attraction = _attractionService.GetById(id)!;
        return Ok(new GetAttractionResponse(attraction));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var attractions = _attractionService.GetAll();
        return Ok(attractions.Select(attraction => new GetAttractionResponse(attraction)).AsQueryable());
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] CreateAttractionRequest model)
    {
        _attractionService.Update(model.ToEntity(id));
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["Operator,"])]
    [HttpPost("{id:int}/entry")]
    public IActionResult ValidateAttractionAccess(int id, [FromBody] AttractionAccessRequest request)
    {
        _attractionService.ValidateAttractionAccess(id, request.TicketQR, request.VisitorNFC);
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["Operator,"])]
    [HttpPost("{id:int}/exit")]
    public IActionResult AttractionExit(int id)
    {
        _attractionService.DecrementCurrentCapacity(id);
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["Operator,"])]
    [HttpPost("{id:int}/Incidences")]
    public IActionResult RegisterIncident(int id, [FromBody] CreateIncidenceRequestByAttractionController request)
    {
        _incidenceService.Create(request.ToEntity(id));
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["Operator,"])]
    [HttpDelete("/Incidences/{Id:int}")]
    public IActionResult DeactivateIncident(int id)
    {
        _incidenceService.DeactivateIncidence(id);
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpGet("{id:int}/visits")]
    public IActionResult GetVisitsBetween(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        var visitsCount = _attractionService.GetVisitsBetween(id, startDate, endDate);
        return Ok(new GetAttractionVisitsResponse()
        {
            AttractionId = id,
            VisitsCount = visitsCount,
            StartDate = startDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            EndDate = endDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
        });
    }
}
