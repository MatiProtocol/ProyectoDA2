using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DateTimeController(IDateTimeService dateTimeController) : Controller, IDateTimeController
{
    private readonly IDateTimeService _dateTimeService = dateTimeController;

    [HttpPost]
    public IActionResult SetDateTime([FromBody] SetDateTimeRequest request)
    {
        _dateTimeService.Now = request.DateTime;
        return Ok();
    }

    [HttpGet]
    public IActionResult GetDateTime()
    {
        return Ok(_dateTimeService.Now);
    }
}
