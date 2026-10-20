using Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Filters;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RewardController(
    IRewardService rewardService,
    IVisitorService visitorService) : ControllerBase, IRewardController
{
    private readonly IRewardService _rewardService = rewardService;
    private readonly IVisitorService _visitorService = visitorService;

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpPost]
    public IActionResult Create([FromBody] CreateRewardRequest model)
    {
        var reward = model.ToEntity();
        _rewardService.Create(reward);
        return Created($"api/reward/{reward.Id}", new GetRewardResponse(reward));
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, CreateRewardRequest model)
    {
        var reward = model.ToEntity(id);
        _rewardService.Update(reward);
        return Ok();
    }

    [Authorize]
    [RolePermissionFilter(["ParkAdmin,"], true)]
    [HttpDelete("{id:int}")]
    public IActionResult Remove(int id)
    {
        _rewardService.Delete(id);
        return Ok();
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var reward = _rewardService.GetById(id)!;
        return Ok(new GetRewardResponse(reward));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var rewards = _rewardService.GetAll();
        return Ok(rewards.Select(r => new GetRewardResponse(r)));
    }

    [Authorize]
    [HttpGet("available/{id:int}")]
    public IActionResult GetAvailableRewards(int id)
    {
        var rewards = _rewardService.GetAvailableRewardsForVisitor(id);
        var visitor = _visitorService.GetVisitorWithParkInformation(id);

        return Ok(new GetAvailableRewardsResponse(
            rewards,
            visitor.VisitorParkInformation.Points,
            visitor.VisitorParkInformation.VisitorMembership));
    }

    [HttpPost("redeem")]
    public IActionResult RedeemReward([FromBody] RedeemRewardRequest request)
    {
        var redemption = _rewardService.RedeemReward(request.VisitorId, request.RewardId);
        var reward = _rewardService.GetById(request.RewardId);
        var visitor = _visitorService.GetVisitorWithParkInformation(request.VisitorId);

        return Ok(new RewardRedemptionResponse(
            redemption,
            reward.Name,
            visitor.VisitorParkInformation.Points));
    }

    [Authorize]
    [OwnAccountOrRoleFilter(["ParkAdmin,"])]
    [HttpGet("redemptions/{id:int}")]
    public IActionResult GetVisitorRedemptions(int id)
    {
        var redemptions = _rewardService.GetVisitorRedemptions(id);
        return Ok(new GetVisitorRedemptionsResponse(redemptions));
    }

    [Authorize]
    [HttpGet("validate/{id:int}/{rewardId:int}")]
    public IActionResult ValidateRedemption(int id, int rewardId)
    {
        var (canRedeem, reason) = _rewardService.ValidateRedemptionWithReason(id, rewardId);
        var visitor = _visitorService.GetVisitorWithParkInformation(id);
        var reward = _rewardService.GetById(rewardId);

        return Ok(new ValidateRedemptionResponse(
            canRedeem,
            visitor?.VisitorParkInformation.Points ?? 0,
            reward?.PointsCost ?? 0,
            reason));
    }
}
