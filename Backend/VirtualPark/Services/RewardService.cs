using Abstraction;
using Domain;
using Exceptions;
using NonDomainAbstractions;

namespace Services;

public class RewardService(
    IRewardRepository rewardRepository,
    IVisitorService visitorService,
    IDateTimeService dateTimeService,
    IPointsHistoryService pointsHistoryService,
    IPunctuationService punctuationService) : IRewardService
{
    private readonly IRewardRepository _rewardRepository = rewardRepository;
    private readonly IVisitorService _visitorService = visitorService;
    private readonly IDateTimeService _dateTimeService = dateTimeService;
    private readonly IPointsHistoryService _pointsHistoryService = pointsHistoryService;
    private readonly IPunctuationService _punctuationService = punctuationService;

    public void Create(Reward entity)
    {
        ValidateReward(entity);
        _rewardRepository.Add(entity);
    }

    public void Delete(int id)
    {
        var reward = _rewardRepository.GetById(id);
        if(reward == null)
        {
            throw new ResourceNotFoundException("Reward", id);
        }

        _rewardRepository.Remove(reward);
    }

    public IEnumerable<Reward> GetAll()
    {
        return _rewardRepository.GetAll();
    }

    public void Update(Reward entity)
    {
        ValidateReward(entity);
        _rewardRepository.Update(entity);
    }

    public Reward GetById(int id)
    {
        var reward = _rewardRepository.GetById(id);
        if(reward == null)
        {
            throw new ResourceNotFoundException("Reward", id);
        }

        return reward;
    }

    public List<Reward> GetAvailableRewardsForVisitor(int visitorId)
    {
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);
        if(visitor == null)
        {
            throw new ResourceNotFoundException("Visitor", visitorId);
        }

        var allRewards = _rewardRepository.GetActiveRewards();
        var visitorMembership = visitor.VisitorParkInformation.VisitorMembership;
        var visitorPoints = visitor.VisitorParkInformation.Points;

        return allRewards
            .Where(r => r.IsAvailable())
            .Where(r => r.CanBeRedeemedByMembership(visitorMembership))
            .Where(r => r.PointsCost <= visitorPoints)
            .ToList();
    }

    public (bool CanRedeem, string? Reason) ValidateRedemptionWithReason(int visitorId, int rewardId)
    {
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);
        var reward = _rewardRepository.GetById(rewardId);

        if(visitor == null)
        {
            return (false, "Visitor not found");
        }

        if(reward == null)
        {
            return (false, "Reward not found");
        }

        if(!reward.IsAvailable())
        {
            return (false, "Reward not available");
        }

        if(visitor.VisitorParkInformation.Points < reward.PointsCost)
        {
            return (false, "Insufficient points");
        }

        if(!reward.CanBeRedeemedByMembership(visitor.VisitorParkInformation.VisitorMembership))
        {
            return (false, "Membership level requirement not met");
        }

        return (true, null);
    }

    public bool ValidateRedemption(int visitorId, int rewardId)
    {
        var (canRedeem, _) = ValidateRedemptionWithReason(visitorId, rewardId);
        return canRedeem;
    }

    public RewardRedemption RedeemReward(int visitorId, int rewardId)
    {
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);
        if(visitor == null)
        {
            throw new ResourceNotFoundException("Visitor", visitorId);
        }

        var reward = _rewardRepository.GetById(rewardId);
        if(reward == null)
        {
            throw new ResourceNotFoundException("Reward", rewardId);
        }

        if(!reward.IsAvailable())
        {
            throw new RewardNotAvailableException();
        }

        if(!reward.CanBeRedeemedByMembership(visitor.VisitorParkInformation.VisitorMembership))
        {
            throw new MembershipLevelRequirementException(
                reward.RequiredMembershipLevel ?? "Any",
                visitor.VisitorParkInformation.VisitorMembership);
        }

        if(visitor.VisitorParkInformation.Points < reward.PointsCost)
        {
            throw new InsufficientPointsException(
                reward.PointsCost,
                visitor.VisitorParkInformation.Points);
        }

        if(!reward.DecrementStock())
        {
            throw new InvalidOperationException("Failed to decrement stock");
        }

        var redemption = new RewardRedemption
        {
            VisitorParkInformationId = visitor.VisitorParkInformation.Id,
            RewardId = rewardId,
            RedemptionDate = _dateTimeService.Now,
            PointsSpent = reward.PointsCost,
            Status = RedemptionStatus.Completed
        };

        visitor.VisitorParkInformation.Points -= reward.PointsCost;
        visitor.VisitorParkInformation.RewardRedemptions.Add(redemption);

        _pointsHistoryService.RecordTransaction(
            visitor.VisitorParkInformation.Id,
            -reward.PointsCost,
            "Reward",
            $"Redeemed: {reward.Name}",
            _punctuationService.GetActiveStrategy(),
            rewardId);

        _rewardRepository.Update(reward);
        _visitorService.UpdateVisitorParkInformation(visitor.VisitorParkInformation);
        _visitorService.Update(visitor);
        return redemption;
    }

    public List<RewardRedemption> GetVisitorRedemptions(int visitorId)
    {
        var visitor = _visitorService.GetVisitorWithParkInformation(visitorId);
        if(visitor == null)
        {
            throw new ResourceNotFoundException("Visitor", visitorId);
        }

        return visitor.VisitorParkInformation.RewardRedemptions;
    }

    public void CancelRedemption(int redemptionId)
    {
        throw new NotImplementedException();
    }

    private void ValidateReward(Reward reward)
    {
        if(string.IsNullOrWhiteSpace(reward.Name))
        {
            throw new InvalidOperationException("Reward name cannot be empty");
        }

        if(reward.PointsCost <= 0)
        {
            throw new InvalidOperationException("Points cost must be positive");
        }

        if(reward.AvailableQuantity < 0)
        {
            throw new InvalidOperationException("Available quantity cannot be negative");
        }
    }
}
