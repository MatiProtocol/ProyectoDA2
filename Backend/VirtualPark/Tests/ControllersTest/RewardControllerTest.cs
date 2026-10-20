using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;

[TestClass]
public class RewardControllerTest
{
    private Mock<IRewardService> _mockRewardService = null!;
    private Mock<IVisitorService> _mockVisitorService = null!;
    private RewardController _controller = null!;
    private Reward _testReward = null!;
    private Visitor _testVisitor = null!;
    private RewardRedemption _testRedemption = null!;

    [TestInitialize]
    public void Initialize()
    {
        _mockRewardService = new Mock<IRewardService>();
        _mockVisitorService = new Mock<IVisitorService>();
        _controller = new RewardController(_mockRewardService.Object, _mockVisitorService.Object);

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
            VisitorParkInformation = new VisitorParkInformation
            {
                Id = 1,
                Points = 2000,
                VisitorMembership = "Premium"
            }
        };

        _testRedemption = new RewardRedemption
        {
            Id = 1,
            VisitorParkInformationId = 1,
            RewardId = 1,
            PointsSpent = 1000,
            Status = RedemptionStatus.Completed,
            RedemptionDate = DateTime.Now
        };
    }

    [TestMethod]
    public void Constructor_WithValidServices_CreatesController()
    {
        Assert.IsNotNull(_controller);
    }

    [TestMethod]
    public void Create_WithValidRequest_ReturnsCreatedResult()
    {
        var request = new CreateRewardRequest
        {
            Name = "Test Reward",
            Description = "Test Description",
            PointsCost = 100,
            AvailableQuantity = 10,
            RequiredMembershipLevel = "Premium"
        };
        _mockRewardService.Setup(x => x.Create(It.IsAny<Reward>()));

        var result = _controller.Create(request);

        Assert.IsInstanceOfType(result, typeof(CreatedResult));
    }

    [TestMethod]
    public void Create_WithValidRequest_CallsRewardServiceCreate()
    {
        var request = new CreateRewardRequest
        {
            Name = "Test Reward",
            Description = "Test Description",
            PointsCost = 100,
            AvailableQuantity = 10
        };
        _mockRewardService.Setup(x => x.Create(It.IsAny<Reward>()));

        _controller.Create(request);

        _mockRewardService.Verify(x => x.Create(It.IsAny<Reward>()), Times.Once);
    }

    [TestMethod]
    public void Create_WithValidRequest_ReturnsGetRewardResponse()
    {
        var request = new CreateRewardRequest
        {
            Name = "Test Reward",
            Description = "Test Description",
            PointsCost = 100,
            AvailableQuantity = 10
        };
        _mockRewardService.Setup(x => x.Create(It.IsAny<Reward>()));

        var result = _controller.Create(request) as CreatedResult;

        Assert.IsInstanceOfType(result!.Value, typeof(GetRewardResponse));
    }

    [TestMethod]
    public void Update_WithValidRequest_ReturnsOkResult()
    {
        var request = new CreateRewardRequest
        {
            Name = "Updated Reward",
            Description = "Updated Description",
            PointsCost = 150,
            AvailableQuantity = 5,
            RequiredMembershipLevel = "VIP"
        };
        _mockRewardService.Setup(x => x.Update(It.IsAny<Reward>()));

        var result = _controller.Update(1, request);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void Update_WithValidRequest_CallsRewardServiceUpdate()
    {
        var request = new CreateRewardRequest
        {
            Name = "Updated Reward",
            Description = "Updated Description",
            PointsCost = 150,
            AvailableQuantity = 5
        };
        _mockRewardService.Setup(x => x.Update(It.IsAny<Reward>()));

        _controller.Update(1, request);

        _mockRewardService.Verify(x => x.Update(It.IsAny<Reward>()), Times.Once);
    }

    [TestMethod]
    public void Remove_WithExistingId_ReturnsOkResult()
    {
        _mockRewardService.Setup(x => x.Delete(1));

        var result = _controller.Remove(1);

        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void Remove_WithExistingId_CallsRewardServiceDelete()
    {
        _mockRewardService.Setup(x => x.Delete(1));

        _controller.Remove(1);

        _mockRewardService.Verify(x => x.Delete(1), Times.Once);
    }

    [TestMethod]
    public void GetById_WithExistingReward_ReturnsOkResult()
    {
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.GetById(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetById_WithExistingReward_ReturnsGetRewardResponse()
    {
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.GetById(1) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(GetRewardResponse));
    }

    [TestMethod]
    public void GetAll_WithRewards_ReturnsOkResult()
    {
        var rewards = new List<Reward> { _testReward };
        _mockRewardService.Setup(x => x.GetAll()).Returns(rewards);

        var result = _controller.GetAll();

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetAll_WithRewards_ReturnsListOfGetRewardResponse()
    {
        var rewards = new List<Reward> { _testReward };
        _mockRewardService.Setup(x => x.GetAll()).Returns(rewards);

        var result = _controller.GetAll() as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(IEnumerable<GetRewardResponse>));
    }

    [TestMethod]
    public void GetAll_WithEmptyList_ReturnsEmptyList()
    {
        _mockRewardService.Setup(x => x.GetAll()).Returns([]);

        var result = _controller.GetAll() as OkObjectResult;
        var responses = (result!.Value as IEnumerable<GetRewardResponse>)?.ToList();

        Assert.AreEqual(0, responses!.Count);
    }

    [TestMethod]
    public void GetAvailableRewards_WithExistingVisitor_ReturnsOkResult()
    {
        var rewards = new List<Reward> { _testReward };
        _mockRewardService.Setup(x => x.GetAvailableRewardsForVisitor(1)).Returns(rewards);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetAvailableRewards(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetAvailableRewards_WithExistingVisitor_ReturnsGetAvailableRewardsResponse()
    {
        var rewards = new List<Reward> { _testReward };
        _mockRewardService.Setup(x => x.GetAvailableRewardsForVisitor(1)).Returns(rewards);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.GetAvailableRewards(1) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(GetAvailableRewardsResponse));
    }

    [TestMethod]
    public void RedeemReward_WithValidRequest_ReturnsOkResult()
    {
        var request = new RedeemRewardRequest { VisitorId = 1, RewardId = 1 };
        _mockRewardService.Setup(x => x.RedeemReward(1, 1)).Returns(_testRedemption);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.RedeemReward(request);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void RedeemReward_WithValidRequest_ReturnsRewardRedemptionResponse()
    {
        var request = new RedeemRewardRequest { VisitorId = 1, RewardId = 1 };
        _mockRewardService.Setup(x => x.RedeemReward(1, 1)).Returns(_testRedemption);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        var result = _controller.RedeemReward(request) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(RewardRedemptionResponse));
    }

    [TestMethod]
    public void RedeemReward_WithValidRequest_CallsRewardServiceRedeemReward()
    {
        var request = new RedeemRewardRequest { VisitorId = 1, RewardId = 1 };
        _mockRewardService.Setup(x => x.RedeemReward(1, 1)).Returns(_testRedemption);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);

        _controller.RedeemReward(request);

        _mockRewardService.Verify(x => x.RedeemReward(1, 1), Times.Once);
    }

    [TestMethod]
    public void GetVisitorRedemptions_WithRedemptions_ReturnsOkResult()
    {
        var redemptions = new List<RewardRedemption> { _testRedemption };
        _mockRewardService.Setup(x => x.GetVisitorRedemptions(1)).Returns(redemptions);

        var result = _controller.GetVisitorRedemptions(1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetVisitorRedemptions_WithRedemptions_ReturnsGetVisitorRedemptionsResponse()
    {
        var redemptions = new List<RewardRedemption> { _testRedemption };
        _mockRewardService.Setup(x => x.GetVisitorRedemptions(1)).Returns(redemptions);

        var result = _controller.GetVisitorRedemptions(1) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(GetVisitorRedemptionsResponse));
    }

    [TestMethod]
    public void GetVisitorRedemptions_WithEmptyList_ReturnsEmptyList()
    {
        _mockRewardService.Setup(x => x.GetVisitorRedemptions(1)).Returns([]);

        var result = _controller.GetVisitorRedemptions(1) as OkObjectResult;
        var response = result!.Value as GetVisitorRedemptionsResponse;

        Assert.AreEqual(0, response!.Redemptions.Count);
    }

    [TestMethod]
    public void ValidateRedemption_WithValidData_ReturnsOkResult()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((true, null));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1);

        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void ValidateRedemption_WithValidData_ReturnsValidateRedemptionResponse()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((true, null));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;

        Assert.IsInstanceOfType(result!.Value, typeof(ValidateRedemptionResponse));
    }

    [TestMethod]
    public void ValidateRedemption_WithTrueValidation_SetsCanRedeemTrue()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((true, null));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.IsTrue(response!.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemption_WithFalseValidation_SetsCanRedeemFalse()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Test reason"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.IsFalse(response!.CanRedeem);
    }

    [TestMethod]
    public void ValidateRedemption_WithNullVisitor_SetsZeroPoints()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Visitor not found"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual(0, response!.VisitorPoints);
    }

    [TestMethod]
    public void ValidateRedemption_WithNullVisitor_SetsZeroVisitorPoints()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Visitor not found"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual(0, response!.VisitorPoints);
    }

    [TestMethod]
    public void ValidateRedemption_WithNullReward_SetsZeroRewardCost()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Reward not found"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns((Reward)null!);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual(0, response!.RewardCost);
    }

    [TestMethod]
    public void ValidateRedemption_WithNullVisitor_SetsReasonVisitorNotFound()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Visitor not found"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns((Visitor)null!);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual("Visitor not found", response!.ReasonIfCannot);
    }

    [TestMethod]
    public void ValidateRedemption_WithNullReward_SetsReasonRewardNotFound()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Reward not found"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns((Reward)null!);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual("Reward not found", response!.ReasonIfCannot);
    }

    [TestMethod]
    public void ValidateRedemption_WithUnavailableReward_SetsReasonRewardNotAvailable()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Reward not available"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual("Reward not available", response!.ReasonIfCannot);
    }

    [TestMethod]
    public void ValidateRedemption_WithInsufficientPoints_SetsReasonInsufficientPoints()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Insufficient points"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual("Insufficient points", response!.ReasonIfCannot);
    }

    [TestMethod]
    public void ValidateRedemption_WithMembershipRequirement_SetsReasonMembershipNotMet()
    {
        _mockRewardService.Setup(x => x.ValidateRedemptionWithReason(1, 1)).Returns((false, "Membership level requirement not met"));
        _mockVisitorService.Setup(x => x.GetVisitorWithParkInformation(1)).Returns(_testVisitor);
        _mockRewardService.Setup(x => x.GetById(1)).Returns(_testReward);

        var result = _controller.ValidateRedemption(1, 1) as OkObjectResult;
        var response = result!.Value as ValidateRedemptionResponse;

        Assert.AreEqual("Membership level requirement not met", response!.ReasonIfCannot);
    }
}
