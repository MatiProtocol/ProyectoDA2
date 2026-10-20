using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface IAttractionController : INotUserController<CreateAttractionRequest>
{
    public IActionResult ValidateAttractionAccess(int id, [FromBody] AttractionAccessRequest request);
    public IActionResult AttractionExit(int id);
    public IActionResult RegisterIncident(int id, [FromBody] CreateIncidenceRequestByAttractionController request);
    public IActionResult DeactivateIncident(int id);
    public IActionResult GetVisitsBetween(int id, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate);
}
