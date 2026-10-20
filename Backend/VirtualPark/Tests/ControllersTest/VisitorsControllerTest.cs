using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;

[TestClass]
public class VisitorsControllerTest
{
    private Mock<IVisitorService> _visitorService = null!;

    private IVisitorController _visitorController = null!;
    private IEnumerable<Visitor> _visitors = null!;

    private Visitor _nullVisitor = null!;
    private Visitor _visitor = null!;
    private CreateVisitorRequest _createVisitorRequest = null!;

    [TestInitialize]
    public void Initialize()
    {
        _visitorService = new Mock<IVisitorService>(MockBehavior.Strict);

        _visitorController = new VisitorsController(_visitorService.Object);

        _visitor = new Visitor
        {
            Id = 1,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            BirthDate = DateTime.Now
        };
        _visitors = new List<Visitor> { _visitor, new() }.AsQueryable();

        _createVisitorRequest = new CreateVisitorRequest
        {
            Name = "Pablito",
            Email = "Example@Example.com",
            OldPassword = "1234ASDe@",
            Birthdate = DateTime.Now
        };

        _nullVisitor = null!;
    }

    [TestMethod]
    public void VisitorController_ItsNotNull()
    {
        Assert.IsNotNull(_visitorController);
    }

    [TestMethod]
    public void AddVisitor_Ok()
    {
        _visitorService.Setup(s => s.Create(It.IsAny<Visitor>()));

        var result = _visitorController.Create(_createVisitorRequest) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void RemoveVisitor_Ok()
    {
        _visitorService.Setup(s => s.Delete(It.IsAny<int>()));

        var result = _visitorController.Remove(_visitor.Id) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void GetVisitor_Ok()
    {
        _visitorService.Setup(s => s.GetById(_visitor.Id)).Returns(_visitor);

        var response = _visitorController.GetById(_visitor.Id) as OkObjectResult;

        var result = response!.Value as GetVisitorResponse;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetVisitor_NotFound()
    {
        _visitorService.Setup(s => s.GetById(_visitor.Id)).Returns(_nullVisitor);

        Assert.ThrowsException<NullReferenceException>(() => _visitorController.GetById(_visitor.Id));
    }

    [TestMethod]
    public void GetAllVisitors_Ok()
    {
        _visitorService.Setup(s => s.GetAll(null)).Returns(_visitors.AsQueryable());

        var response = _visitorController.GetAll() as OkObjectResult;

        var result = response!.Value as IEnumerable<GetVisitorResponse>;

        Assert.AreEqual(2, result!.Count());
    }

    [TestMethod]
    public void UpdateVisitor_Ok()
    {
        _visitorService.Setup(s => s.Update(It.IsAny<Visitor>(), _createVisitorRequest.OldPassword));

        var result = _visitorController.Update(_visitor.Id, _createVisitorRequest) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void GetVisitorNFC_Ok()
    {
        _visitorService.Setup(s => s.GetVisitorNFC(_visitor.Id))
            .Returns(_visitor.VisitorParkInformation.NFC);

        var response = _visitorController.GetVisitorNFC(_visitor.Id) as OkObjectResult;

        var result = response!.Value is Guid;

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void GetAvailableTickets_Ok()
    {
        _visitorService.Setup(vs => vs.GetAvailableTicketsById(_visitor.Id)).Returns(new List<Ticket>().AsQueryable());

        var response = _visitorController.GetAvailableTicketsById(_visitor.Id) as OkObjectResult;

        Assert.IsInstanceOfType<OkObjectResult>(response);
    }
}
