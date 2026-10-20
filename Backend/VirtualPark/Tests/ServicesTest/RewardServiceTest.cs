using Abstraction;
using Domain;
using Exceptions;
using Moq;
using NonDomainAbstractions;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class RewardServiceTest
{
    private Mock<IRewardRepository> _mockRewardRepository = null!;
    private Mock<IVisitorService> _mockVisitorService = null!;
    private Mock<IDateTimeService> _mockDateTimeService = null!;
    private Mock<IPointsHistoryService> _mockPointsHistoryService = null!;
    private Mock<IPunctuationService> _mockPunctuationService = null!;
    private IRewardService _rewardService = null!;
    private Reward _testReward = null!;
    private Visitor _testVisitor = null!;
    private DateTime _testDate;

    [TestInitialize]
    public void Initialize()
    {
        _mockRewardRepository = new Mock<IRewardRepository>();
        _mockVisitorService = new Mock<IVisitorService>();
        _mockDateTimeService = new Mock<IDateTimeService>();
        _mockPointsHistoryService = new Mock<IPointsHistoryService>();
        _mockPunctuationService = new Mock<IPunctuationService>();
        _testDate = new DateTime(2025, 10, 15);
        _mockDateTimeService.Setup(x => x.Now).Returns(_testDate);
        _rewardService = new RewardService(
            _mockRewardRepository.Object,
            _mockVisitorService.Object,
            _mockDateTimeService.Object,
            _mockPointsHistoryService.Object,
            _mockPunctuationService.Object);

        _testReward = new Reward
        {
            Id = 1,
            Name = "Free Ticket",
            Description = "One free entrance",
            PointsCost = 1000,
            AvailableQuantity = 10,
            RequiredMembershipLevel = "Premium",
            Active = true
        };

        _testVisitor = new Visitor
        {
            Id = 1,
            Name = "John Doe",
            Email = "john@test.com",
            BirthDate = new DateTime(1990, 1, 1),
            Password = "password123",
            VisitorParkInformation = new VisitorParkInformation
            {
                Id = 1,
                Points = 2000,
                VisitorMembership = "Premium",
                RewardRedemptions = [],
                PointsHistory = []
            }
        };
    }

    [TestMethod]
    public void CreateRewardService_IsNotNull()
    {
        Assert.IsNotNull(_rewardService);
    }

    [TestMethod]
    public void Create_WithValidReward_CallsRepositoryAdd()
    {
        _rewardService.Create(_testReward);
        _mockRewardRepository.Verify(x => x.Add(_testReward), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithEmptyName_ThrowsException()
    {
        _testReward.Name = string.Empty;
        _rewardService.Create(_testReward);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithNullName_ThrowsException()
    {
        _testReward.Name = null!;
        _rewardService.Create(_testReward);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithWhitespaceName_ThrowsException()
    {
        _testReward.Name = "   ";
        _rewardService.Create(_testReward);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithZeroPointsCost_ThrowsException()
    {
        _testReward.PointsCost = 0;
        _rewardService.Create(_testReward);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithNegativePointsCost_ThrowsException()
    {
        _testReward.PointsCost = -100;
        _rewardService.Create(_testReward);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Create_WithNegativeQuantity_ThrowsException()
    {
        _testReward.AvailableQuantity = -1;
        _rewardService.Create(_testReward);
    }

    [TestMethod]
    public void Create_WithZeroQuantity_CallsRepositoryAdd()
    {
        _testReward.AvailableQuantity = 0;
        _rewardService.Create(_testReward);
        _mockRewardRepository.Verify(x => x.Add(_testReward), Times.Once);
    }

    [TestMethod]
    public void Delete_WithExistingReward_CallsRepositoryRemove()
    {
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _rewardService.Delete(1);
        _mockRewardRepository.Verify(x => x.Remove(_testReward), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ResourceNotFoundException))]
    public void Delete_WithNonExistingReward_ThrowsException()
    {
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns((Reward)null!);
        _rewardService.Delete(1);
    }

    [TestMethod]
    public void GetById_WithExistingReward_ReturnsReward()
    {
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.GetById(1);
        Assert.AreEqual(_testReward, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ResourceNotFoundException))]
    public void GetById_WithNonExistingReward_ThrowsException()
    {
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns((Reward)null!);
        _rewardService.GetById(1);
    }

    [TestMethod]
    public void Update_WithValidReward_CallsRepositoryUpdate()
    {
        _rewardService.Update(_testReward);
        _mockRewardRepository.Verify(x => x.Update(_testReward), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Update_WithEmptyName_ThrowsException()
    {
        _testReward.Name = string.Empty;
        _rewardService.Update(_testReward);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Update_WithZeroPointsCost_ThrowsException()
    {
        _testReward.PointsCost = 0;
        _rewardService.Update(_testReward);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Update_WithNegativeQuantity_ThrowsException()
    {
        _testReward.AvailableQuantity = -5;
        _rewardService.Update(_testReward);
    }

    [TestMethod]
    public void GetAll_ReturnsAllRewards()
    {
        var rewards = new List<Reward> { _testReward };
        _mockRewardRepository.Setup(x => x.GetAll()).Returns(rewards);
        var result = _rewardService.GetAll();
        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetAll_WithEmptyRepository_ReturnsEmptyList()
    {
        _mockRewardRepository.Setup(x => x.GetAll()).Returns([]);
        var result = _rewardService.GetAll();
        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void GetAvailableRewardsForVisitor_WithValidVisitor_ReturnsFilteredRewards()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetActiveRewards()).Returns([_testReward]);
        var result = _rewardService.GetAvailableRewardsForVisitor(1);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ResourceNotFoundException))]
    public void GetAvailableRewardsForVisitor_WithNonExistingVisitor_ThrowsException()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        _rewardService.GetAvailableRewardsForVisitor(1);
    }

    [TestMethod]
    public void GetAvailableRewardsForVisitor_FiltersInactiveRewards()
    {
        _testReward.Active = false;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetActiveRewards()).Returns([_testReward]);
        var result = _rewardService.GetAvailableRewardsForVisitor(1);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetAvailableRewardsForVisitor_FiltersOutOfStockRewards()
    {
        _testReward.AvailableQuantity = 0;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetActiveRewards()).Returns([_testReward]);
        var result = _rewardService.GetAvailableRewardsForVisitor(1);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetAvailableRewardsForVisitor_FiltersWrongMembershipRewards()
    {
        _testReward.RequiredMembershipLevel = "VIP";
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetActiveRewards()).Returns([_testReward]);
        var result = _rewardService.GetAvailableRewardsForVisitor(1);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetAvailableRewardsForVisitor_FiltersExpensiveRewards()
    {
        _testReward.PointsCost = 3000;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetActiveRewards()).Returns([_testReward]);
        var result = _rewardService.GetAvailableRewardsForVisitor(1);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    public void GetAvailableRewardsForVisitor_WithNullMembershipRequired_ReturnsReward()
    {
        _testReward.RequiredMembershipLevel = null;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetActiveRewards()).Returns([_testReward]);
        var result = _rewardService.GetAvailableRewardsForVisitor(1);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetAvailableRewardsForVisitor_WithExactPoints_ReturnsReward()
    {
        _testVisitor.VisitorParkInformation.Points = 1000;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetActiveRewards()).Returns([_testReward]);
        var result = _rewardService.GetAvailableRewardsForVisitor(1);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void ValidateRedemption_WithValidConditions_ReturnsTrue()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemption(1, 1);
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void ValidateRedemption_WithNullVisitor_ReturnsFalse()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        var result = _rewardService.ValidateRedemption(1, 1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateRedemption_WithNullReward_ReturnsFalse()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns((Reward)null!);
        var result = _rewardService.ValidateRedemption(1, 1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateRedemption_WithInactiveReward_ReturnsFalse()
    {
        _testReward.Active = false;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemption(1, 1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateRedemption_WithNoStock_ReturnsFalse()
    {
        _testReward.AvailableQuantity = 0;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemption(1, 1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateRedemption_WithWrongMembership_ReturnsFalse()
    {
        _testReward.RequiredMembershipLevel = "VIP";
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemption(1, 1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateRedemption_WithInsufficientPoints_ReturnsFalse()
    {
        _testVisitor.VisitorParkInformation.Points = 500;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemption(1, 1);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateRedemptionWithReason_WithValidConditions_ReturnsTrue()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemptionWithReason(1, 1);
        Assert.IsTrue(result.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemptionWithReason_WithNullVisitor_ReturnsFalse()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        var result = _rewardService.ValidateRedemptionWithReason(1, 1);
        Assert.IsFalse(result.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemptionWithReason_WithNullReward_ReturnsFalse()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns((Reward)null!);
        var result = _rewardService.ValidateRedemptionWithReason(1, 1);
        Assert.IsFalse(result.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemptionWithReason_WithInactiveReward_ReturnsFalse()
    {
        _testReward.Active = false;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemptionWithReason(1, 1);
        Assert.IsFalse(result.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemptionWithReason_WithNoStock_ReturnsFalse()
    {
        _testReward.AvailableQuantity = 0;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemptionWithReason(1, 1);
        Assert.IsFalse(result.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemptionWithReason_WithWrongMembership_ReturnsFalse()
    {
        _testReward.RequiredMembershipLevel = "VIP";
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemptionWithReason(1, 1);
        Assert.IsFalse(result.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemptionWithReason_WithInsufficientPoints_ReturnsFalse()
    {
        _testVisitor.VisitorParkInformation.Points = 500;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        var result = _rewardService.ValidateRedemptionWithReason(1, 1);
        Assert.IsFalse(result.CanRedeem);
    }

    [TestMethod]
    public void RedeemReward_WithValidConditions_ReturnsRedemption()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        var result = _rewardService.RedeemReward(1, 1);
        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void RedeemReward_SetsCorrectRedemptionStatus()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        var result = _rewardService.RedeemReward(1, 1);
        Assert.AreEqual(RedemptionStatus.Completed, result.Status);
    }

    [TestMethod]
    public void RedeemReward_DecrementsVisitorPoints()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        _rewardService.RedeemReward(1, 1);
        Assert.AreEqual(1000, _testVisitor.VisitorParkInformation.Points);
    }

    [TestMethod]
    public void RedeemReward_DecrementsRewardStock()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        _rewardService.RedeemReward(1, 1);
        Assert.AreEqual(9, _testReward.AvailableQuantity);
    }

    [TestMethod]
    public void RedeemReward_AddsRedemptionToVisitor()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        _rewardService.RedeemReward(1, 1);
        Assert.AreEqual(1, _testVisitor.VisitorParkInformation.RewardRedemptions.Count);
    }

    [TestMethod]
    public void RedeemReward_SetsCorrectRedemptionDate()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        var result = _rewardService.RedeemReward(1, 1);
        Assert.AreEqual(_testDate, result.RedemptionDate);
    }

    [TestMethod]
    public void RedeemReward_SetsCorrectPointsSpent()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        var result = _rewardService.RedeemReward(1, 1);
        Assert.AreEqual(1000, result.PointsSpent);
    }

    [TestMethod]
    public void RedeemReward_CallsPointsHistoryService()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        _rewardService.RedeemReward(1, 1);
        _mockPointsHistoryService.Verify(x => x.RecordTransaction(
            1,
            -1000,
            "Reward",
            "Redeemed: Free Ticket",
            "AttractionBased",
            1), Times.Once);
    }

    [TestMethod]
    public void RedeemReward_CallsRewardRepositoryUpdate()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        _rewardService.RedeemReward(1, 1);
        _mockRewardRepository.Verify(x => x.Update(_testReward), Times.Once);
    }

    [TestMethod]
    public void RedeemReward_CallsVisitorServiceUpdate()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockPunctuationService.Setup(x => x.GetActiveStrategy()).Returns("AttractionBased");
        _rewardService.RedeemReward(1, 1);
        _mockVisitorService.Verify(x => x.UpdateVisitorParkInformation(
            _testVisitor.VisitorParkInformation), Times.Once);
    }

    [TestMethod]
    [ExpectedException(typeof(ResourceNotFoundException))]
    public void RedeemReward_WithNonExistingVisitor_ThrowsException()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        _rewardService.RedeemReward(1, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(ResourceNotFoundException))]
    public void RedeemReward_WithNonExistingReward_ThrowsException()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns((Reward)null!);
        _rewardService.RedeemReward(1, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(RewardNotAvailableException))]
    public void RedeemReward_WithInactiveReward_ThrowsException()
    {
        _testReward.Active = false;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _rewardService.RedeemReward(1, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(MembershipLevelRequirementException))]
    public void RedeemReward_WithWrongMembership_ThrowsException()
    {
        _testReward.RequiredMembershipLevel = "VIP";
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _rewardService.RedeemReward(1, 1);
    }

    [TestMethod]
    [ExpectedException(typeof(InsufficientPointsException))]
    public void RedeemReward_WithInsufficientPoints_ThrowsException()
    {
        _testVisitor.VisitorParkInformation.Points = 500;
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardRepository.Setup(x => x.GetById(1)).Returns(_testReward);
        _rewardService.RedeemReward(1, 1);
    }

    [TestMethod]
    public void GetVisitorRedemptions_ReturnsRedemptions()
    {
        var redemption = new RewardRedemption { Id = 1 };
        _testVisitor.VisitorParkInformation.RewardRedemptions.Add(redemption);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        var result = _rewardService.GetVisitorRedemptions(1);
        Assert.AreEqual(1, result.Count);
    }

    [TestMethod]
    public void GetVisitorRedemptions_WithNoRedemptions_ReturnsEmptyList()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        var result = _rewardService.GetVisitorRedemptions(1);
        Assert.AreEqual(0, result.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(ResourceNotFoundException))]
    public void GetVisitorRedemptions_WithNonExistingVisitor_ThrowsException()
    {
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        _rewardService.GetVisitorRedemptions(1);
    }

    [TestMethod]
    [ExpectedException(typeof(NotImplementedException))]
    public void CancelRedemption_ThrowsNotImplemented()
    {
        _rewardService.CancelRedemption(1);
    }
}
