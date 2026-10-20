using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface IPointsHistoryController
{
    IActionResult GetVisitorHistory(int visitorId);
    IActionResult GetHistoryByDateRange([FromBody] GetPointsHistoryRequest request);
    IActionResult GetPointsSummary(int visitorId);
}
