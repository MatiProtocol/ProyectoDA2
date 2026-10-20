using Abstraction;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace Tests.ControllersTest;
[TestClass]
public class WorkersControllerTest
{
    private Mock<IWorkerService> _workerService = null!;

    private WorkersController _workerController = null!;
    private IEnumerable<Worker> _workers = null!;

    private Worker _nullWorker = null!;
    private Worker _worker = null!;
    private CreateWorkerRequest _operatorRequest = null!;
    private CreateWorkerRequest _createWorkerRequest = null!;

    [TestInitialize]
    public void Initialize()
    {
        _workerService = new Mock<IWorkerService>(MockBehavior.Strict);
        _workerController = new WorkersController(_workerService.Object);

        _worker = new Worker
        {
            Id = 1,
            Name = "Jose",
            Email = "example@jose.com",
            Password = "1234dsAe@",
        };
        _workers = new List<Worker> { _worker }.AsQueryable();

        _operatorRequest = new CreateWorkerRequest
        {
            Name = "Pablito",
            Email = "Example@Example.com",
            OldPassword = "1234ADDae@",
            Roles = string.Empty
        };

        _createWorkerRequest = new CreateWorkerRequest
        {
            Name = "Ignacio",
            Email = "Example@google.com",
            OldPassword = "1244RCmDe@",
            Roles = string.Empty
        };

        _nullWorker = null!;
    }

    [TestMethod]
    public void WorkerController_IsNotNull()
    {
        Assert.IsNotNull(_workerController);
    }

    [TestMethod]
    public void AddWorkerParkAdmin_Ok()
    {
        _workerService.Setup(s => s.Create(It.IsAny<Worker>()));

        var result = _workerController.Create(_createWorkerRequest) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void AddWorkerOperator_Ok()
    {
        _workerService.Setup(s => s.Create(It.IsAny<Worker>()));

        var result = _workerController.Create(_operatorRequest) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void RemoveWorker_Ok()
    {
        _workerService.Setup(s => s.Delete(It.IsAny<int>()));

        var result = _workerController.Remove(_worker.Id) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void RemoveWorker_WithNonTrackedWorker_Ok()
    {
        _workerService.Setup(s => s.Delete(It.IsAny<int>()));

        var result = _workerController.Remove(_worker.Id) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void GetWorker_Ok()
    {
        _workerService.Setup(s => s.GetById(_worker.Id)).Returns(_worker);

        var response = _workerController.GetById(_worker.Id) as OkObjectResult;

        var result = response!.Value as GetWorkerResponse;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetWorker_NotFound()
    {
        _workerService.Setup(s => s.GetById(_worker.Id)).Returns(_nullWorker);

        Assert.ThrowsException<NullReferenceException>(() => _workerController.GetById(_worker.Id));
    }

    [TestMethod]
    public void GetAllWorkerWithEmailParam_Ok()
    {
        var workerList = new List<Worker> { _worker };
        _workerService.Setup(s => s.GetAll(_worker.Email)).Returns(workerList);

        var response = _workerController.GetAll(_worker.Email) as OkObjectResult;

        var result = response!.Value as IEnumerable<GetWorkerResponse>;

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void GetAllWorkers_Ok()
    {
        _workerService.Setup(s => s.GetAll(null)).Returns(_workers.AsQueryable());

        var response = _workerController.GetAll() as OkObjectResult;

        var result = response!.Value as IEnumerable<GetWorkerResponse>;

        Assert.AreEqual(1, result!.Count());
    }

    [TestMethod]
    public void UpdateWorker_Ok()
    {
        _workerService.Setup(s => s.Update(It.IsAny<Worker>(), _createWorkerRequest.OldPassword));

        var result = _workerController.Update(_worker.Id, _createWorkerRequest) as OkResult;

        Assert.IsInstanceOfType<OkResult>(result);
    }
}
