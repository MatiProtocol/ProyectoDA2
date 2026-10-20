using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PunctuationsController(
    IPunctuationService punctuationService,
    IDateTimeService dateTimeService) : ControllerBase
{
    private readonly IPunctuationService _punctuationService = punctuationService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpPost]
    public IActionResult SetStrategy([FromBody] SetStrategyRequest request)
    {
        _punctuationService.SetActiveStrategy(request.StrategyName);
        return Ok();
    }

    [HttpGet("active")]
    public IActionResult GetActiveStrategy()
    {
        var strategy = _punctuationService.GetActiveStrategy();
        return Ok(new GetActiveStrategyResponse(strategy));
    }

    [HttpGet]
    public IActionResult GetAvailableStrategies()
    {
        var strategies = _punctuationService.GetAvailableStrategies();
        return Ok(new GetAvailableStrategiesResponse(strategies));
    }

    [Authorize]
    [HttpGet("visitor/{visitorId:int}")]
    public IActionResult GetVisitorPoints(int visitorId)
    {
        var points = _punctuationService.GetVisitorTotalPoints(visitorId, null);
        return Ok(new GetVisitorPointsResponse(visitorId, points));
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpGet("ranking/daily")]
    public IActionResult GetDailyRanking()
    {
        var ranking = _punctuationService.GetDailyRanking();
        return Ok(new GetDailyRankingResponse(ranking, _dateTimeService.Now.Date));
    }

    [Authorize]
    [RolePermissionFilter(["Operator,", "ParkAdmin,"])]
    [HttpPost("access")]
    public IActionResult ProcessAttractionAccess([FromBody] ProcessAttractionAccessRequest request)
    {
        var pointsAwarded = _punctuationService.ProcessAttractionPoints(
            request.VisitorId,
            request.AttractionId, request.VisitorNFC);

        var totalPoints = _punctuationService.GetVisitorTotalPoints(request.VisitorId, request.VisitorNFC);

        return Ok(new ProcessAccessResponse(request.VisitorId, pointsAwarded, totalPoints));
    }
}
