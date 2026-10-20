using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface IRewardController : INotUserController<CreateRewardRequest>
{
    IActionResult GetAvailableRewards(int id);
    IActionResult RedeemReward([FromBody] RedeemRewardRequest request);
    IActionResult GetVisitorRedemptions(int id);
    IActionResult ValidateRedemption(int id, int rewardId);
}
