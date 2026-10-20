using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;

namespace Tests.ControllersTest;

[TestClass]
public class SpecialEventsControllerTest
{
    private Mock<ISpecialEventService> _mockService = null!;
    private SpecialEventsController _controller = null!;
    private CreateSpecialEventRequest _validRequest = null!;
    private SpecialEvent _specialEvent = null!;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ISpecialEventService>();
        _controller = new SpecialEventsController(_mockService.Object);
        _validRequest = CreateValidRequest();
        _specialEvent = CreateSpecialEvent();
    }

    private CreateSpecialEventRequest CreateValidRequest()
    {
        return new CreateSpecialEventRequest
        {
            Name = "Test Event",
            Date = DateTime.Today.AddDays(10),
            StartTime = "18:00",
            EndTime = "23:00",
            MaxCapacity = 100,
            AdditionalCost = 20,
            AttractionsIds = []
        };
    }

    private SpecialEvent CreateSpecialEvent()
    {
        return new SpecialEvent
        {
            Id = 1,
            Name = "Test Event",
            Date = DateTime.Today.AddDays(10),
            StartTime = new TimeSpan(18, 0, 0),
            EndTime = new TimeSpan(23, 0, 0),
            MaxCapacity = 100,
            CurrentCapacity = 50,
            AdditionalCost = 20,
            IncludedAttractions = [new Attraction { Id = 1 }, new Attraction { Id = 2 }],
            Active = true
        };
    }

    [TestMethod]
    public void Create_ReturnsOk()
    {
        var result = _controller.Create(_validRequest);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void Create_CallsServiceCreate()
    {
        _controller.Create(_validRequest);
        _mockService.Verify(x => x.Create(It.IsAny<SpecialEvent>()), Times.Once);
    }

    [TestMethod]
    public void Remove_ReturnsOk()
    {
        var result = _controller.Remove(1);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void Remove_CallsServiceDeactivateEvent()
    {
        _controller.Remove(1);
        _mockService.Verify(x => x.DeactivateEvent(1), Times.Once);
    }

    [TestMethod]
    public void GetById_ReturnsOkWithEvent()
    {
        _mockService.Setup(x => x.GetById(1)).Returns(_specialEvent);
        var result = _controller.GetById(1);
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetAll_ReturnsOkWithEvents()
    {
        var events = new List<SpecialEvent> { _specialEvent };
        _mockService.Setup(x => x.GetAll()).Returns(events);
        var result = _controller.GetAll();
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetById_CallsServiceGetById()
    {
        _mockService.Setup(x => x.GetById(1)).Returns(_specialEvent);
        _controller.GetById(1);
        _mockService.Verify(x => x.GetById(1), Times.Once);
    }

    [TestMethod]
    public void GetById_ReturnsCorrectEvent()
    {
        _mockService.Setup(x => x.GetById(1)).Returns(_specialEvent);
        var result = _controller.GetById(1) as OkObjectResult;
        Assert.IsNotNull(result);
        Assert.IsNotNull(result.Value);
    }

    [TestMethod]
    public void GetAll_CallsServiceGetAll()
    {
        var events = new List<SpecialEvent> { _specialEvent };
        _mockService.Setup(x => x.GetAll()).Returns(events);
        _controller.GetAll();
        _mockService.Verify(x => x.GetAll(), Times.Once);
    }

    [TestMethod]
    public void GetAll_ReturnsCorrectNumberOfEvents()
    {
        var events = new List<SpecialEvent> { _specialEvent, new SpecialEvent() };
        _mockService.Setup(x => x.GetAll()).Returns(events);
        var result = _controller.GetAll() as OkObjectResult;
        var response = result?.Value as IQueryable<object>;
        Assert.AreEqual(2, response?.Count());
    }

    [TestMethod]
    public void GetActiveEvents_ReturnsOkWithActiveEvents()
    {
        var events = new List<SpecialEvent> { _specialEvent };
        _mockService.Setup(x => x.GetActiveEvents()).Returns(events);
        var result = _controller.GetActiveEvents();
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
    }

    [TestMethod]
    public void GetActiveEvents_CallsServiceGetActiveEvents()
    {
        var events = new List<SpecialEvent> { _specialEvent };
        _mockService.Setup(x => x.GetActiveEvents()).Returns(events);
        _controller.GetActiveEvents();
        _mockService.Verify(x => x.GetActiveEvents(), Times.Once);
    }

    [TestMethod]
    public void GetActiveEvents_ReturnsCorrectNumberOfActiveEvents()
    {
        var events = new List<SpecialEvent> { _specialEvent, new SpecialEvent { Active = true } };
        _mockService.Setup(x => x.GetActiveEvents()).Returns(events);
        var result = _controller.GetActiveEvents() as OkObjectResult;
        var response = result?.Value as IQueryable<object>;
        Assert.AreEqual(2, response?.Count());
    }

    [TestMethod]
    public void Update_ReturnsOk()
    {
        var result = _controller.Update(1, _validRequest);
        Assert.IsInstanceOfType(result, typeof(OkResult));
    }

    [TestMethod]
    public void Update_CallsServiceWithCorrectId()
    {
        _controller.Update(1, _validRequest);
        _mockService.Verify(x => x.Update(It.Is<SpecialEvent>(e => e.Id == 1)), Times.Once);
    }

    [TestMethod]
    public void Constructor_InitializesServiceCorrectly()
    {
        var controller = new SpecialEventsController(_mockService.Object);
        Assert.IsNotNull(controller);
    }

    [TestMethod]
    public void Create_WithValidRequest_MapsCorrectlyToEntity()
    {
        SpecialEvent capturedEvent = null!;
        _mockService.Setup(x => x.Create(It.IsAny<SpecialEvent>()))
            .Callback<SpecialEvent>(e => capturedEvent = e);
        _controller.Create(_validRequest);
        Assert.IsNotNull(capturedEvent);
        Assert.AreEqual(_validRequest.Name, capturedEvent.Name);
        Assert.AreEqual(_validRequest.MaxCapacity, capturedEvent.MaxCapacity);
    }

    [TestMethod]
    public void Update_WithValidRequest_MapsCorrectlyToEntity()
    {
        SpecialEvent capturedEvent = null!;
        _mockService.Setup(x => x.Update(It.IsAny<SpecialEvent>()))
            .Callback<SpecialEvent>(e => capturedEvent = e);
        _controller.Update(1, _validRequest);
        Assert.IsNotNull(capturedEvent);
        Assert.AreEqual(1, capturedEvent.Id);
        Assert.AreEqual(_validRequest.Name, capturedEvent.Name);
    }
}
