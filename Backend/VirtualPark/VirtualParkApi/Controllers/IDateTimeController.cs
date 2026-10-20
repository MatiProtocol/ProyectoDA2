using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface IDateTimeController
{
    IActionResult SetDateTime(SetDateTimeRequest request);
    IActionResult GetDateTime();
}
