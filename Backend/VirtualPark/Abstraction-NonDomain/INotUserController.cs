using Microsoft.AspNetCore.Mvc;

namespace NonDomainAbstractions;

public interface INotUserController<T> : IController<T>
    where T : class
{
    IActionResult GetAll();
}
