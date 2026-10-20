using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface IIncidenceController : INotUserController<CreateIncidenceRequest>
{
    public IActionResult GetActiveIncidences();
    public IActionResult GetByAttractionId(int attractionId);
    public IActionResult DeactivateIncidence(int id);
    public IActionResult ActivateIncidence(int id);
}
