using Microsoft.AspNetCore.Mvc;

namespace NonDomainAbstractions;

public interface IController<in T>
    where T : class
{
    IActionResult Create(T model);

    IActionResult Remove(int id);

    IActionResult GetById(int id);

    IActionResult Update(int id, T model);
}
