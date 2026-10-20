using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PointsHistoryController(
    IPointsHistoryService pointsHistoryService,
    IVisitorService visitorService) : ControllerBase, IPointsHistoryController
{
    private readonly IPointsHistoryService _pointsHistoryService = pointsHistoryService;
    private readonly IVisitorService _visitorService = visitorService;

    [HttpGet("{visitorId:int}")]
    public IActionResult GetVisitorHistory(int visitorId)
    {
        var history = _pointsHistoryService.GetVisitorHistory(visitorId);
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);

        if(visitor == null)
        {
            return NotFound("Visitor not found");
        }

        return Ok(new GetPointsHistoryResponse(
            history,
            visitor.VisitorParkInformation.Points));
    }

    [Authorize]
    [OwnAccountOrRoleFilter(["ParkAdmin,"])]
    [HttpPost("range")]
    public IActionResult GetHistoryByDateRange([FromBody] GetPointsHistoryRequest request)
    {
        var startDate = request.StartDate ?? DateTime.MinValue;
        var endDate = request.EndDate ?? DateTime.Now;

        var history = _pointsHistoryService.GetHistoryByDateRange(
            request.VisitorId,
            startDate,
            endDate);

        var visitor = _visitorService.GetVisitorWithParkInformation(request.VisitorId);

        if(visitor == null)
        {
            return NotFound("Visitor not found");
        }

        return Ok(new GetPointsHistoryResponse(
            history,
            visitor.VisitorParkInformation.Points));
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpGet("summary/{visitorId:int}")]
    public IActionResult GetPointsSummary(int visitorId)
    {
        var history = _pointsHistoryService.GetVisitorHistory(visitorId);
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);

        if(visitor == null)
        {
            return NotFound("Visitor not found");
        }

        var totalEarned = history.Where(h => h.Points > 0).Sum(h => h.Points);
        var totalSpent = Math.Abs(history.Where(h => h.Points < 0).Sum(h => h.Points));

        return Ok(new PointsSummaryResponse
        {
            VisitorId = visitorId,
            TotalPointsEarned = totalEarned,
            TotalPointsSpent = totalSpent,
            CurrentBalance = visitor.VisitorParkInformation.Points,
            TodayPoints = visitor.VisitorParkInformation.TodayPoints,
            RedemptionsToday = visitor.VisitorParkInformation.RedeemPointsToday
        });
    }
}
