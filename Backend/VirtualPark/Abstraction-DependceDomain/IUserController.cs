using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;

namespace Abstraction;

public interface IUserController<T> : IController<T>
    where T : class
{
    public IActionResult GetAll([FromQuery] string? email = null);
}
