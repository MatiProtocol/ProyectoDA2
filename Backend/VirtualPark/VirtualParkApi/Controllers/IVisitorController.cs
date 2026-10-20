using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface IVisitorController : IController<CreateVisitorRequest>
{
    public IActionResult GetAll(string? email = null);
    public IActionResult GetVisitorNFC(int id);
    public IActionResult GetAvailableTicketsById(int id);
}
