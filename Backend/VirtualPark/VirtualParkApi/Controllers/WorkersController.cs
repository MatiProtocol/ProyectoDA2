using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkersController(IWorkerService workerService) : ControllerBase, IUserController<CreateWorkerRequest>
{
    private readonly IWorkerService _workerService = workerService;

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"])]
    [HttpPost]
    public IActionResult Create(CreateWorkerRequest model)
    {
        _workerService.Create(model.ToEntity());
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"])]
    [HttpDelete("{id:int}")]
    public IActionResult Remove(int id)
    {
        _workerService.Delete(id);
        return Ok();
    }

    [Authorize]
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        return Ok(new GetWorkerResponse(_workerService.GetById(id)!));
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,", "Operator,"])]
    [HttpGet]
    public IActionResult GetAll([FromQuery] string? email = null)
    {
        var workers = _workerService.GetAll(email);
        return Ok(workers.Select(worker => new GetWorkerResponse(worker)));
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,", "Operator,"])]
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, CreateWorkerRequest model)
    {
        _workerService.Update(model.ToEntity(id), model.OldPassword);
        return Ok();
    }
}
