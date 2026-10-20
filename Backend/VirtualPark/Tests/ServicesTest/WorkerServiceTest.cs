using Abstraction;
using Domain;
using Exceptions;
using Moq;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class WorkerServiceTest
{
    private Mock<IWorkerRepository> _repository = null!;
    private WorkerService _workerService = null!;
    private Worker _parkAdmin = null!;
    private Worker? _nullWorker;

    [TestInitialize]
    public void Initialize()
    {
        _repository = new Mock<IWorkerRepository>(MockBehavior.Strict);
        _workerService = new WorkerService(_repository.Object);

        _parkAdmin = new Worker()
        {
            Id = 1,
            Name = "Pablito",
            Email = "example@example.com",
            Password = "1234ASDe@",
            Roles = nameof(ValidRoles.ParkAdmin) + ","
        };
        _nullWorker = null;
    }

    [TestMethod]
    public void CreateWorkerService_IsNotNull()
    {
        Assert.IsNotNull(_workerService);
    }

    [TestMethod]
    public void CreateWorker_RepeatedEmail_ThrowDbCreatedUniqueOrKeyException()
    {
        _repository.Setup(r => r.GetByEmail(_parkAdmin.Email)).Returns(_parkAdmin);
        _repository.Setup(r => r.Add(It.IsAny<Worker>()));

        Assert.ThrowsException<DbCreatedUniqueOrKeyException>(() => _workerService.Create(_parkAdmin));
    }

    [TestMethod]
    public void InsertCharacter_Ok()
    {
        _parkAdmin.Roles = nameof(ValidRoles.ParkAdmin);
        _repository.Setup(r => r.GetByEmail(It.IsAny<string>())).Returns(_nullWorker);
        _repository.Setup(r => r.Add(It.IsAny<Worker>()));
        _workerService.Create(_parkAdmin);

        _repository.VerifyAll();
    }

    [TestMethod]
    public void InsertCharacter_InvalidRole_throwInvalidOperationException()
    {
        _parkAdmin.Roles = "InvalidRole";
        _repository.Setup(r => r.GetByEmail(_parkAdmin.Email)).Returns(_nullWorker);
        _repository.Setup(r => r.Add(It.IsAny<Worker>()));
        Assert.ThrowsException<InvalidOperationException>(() => _workerService.Create(_parkAdmin));
    }

    [TestMethod]
    public void RemoveCharacter_Ok()
    {
        _repository.Setup(r => r.Remove(It.IsAny<Worker>()));

        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);

        _workerService.Delete(_parkAdmin.Id);

        _repository.VerifyAll();
    }

    [TestMethod]
    public void RemoveCharacter_ThrowException()
    {
        _repository.Setup(r => r.Remove(It.IsAny<Worker>()));
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_nullWorker);

        string? result = null;

        try
        {
            _workerService.Delete(_parkAdmin.Id);
        }
        catch(Exception e)
        {
            result = e.Message;
        }

        Assert.AreEqual("No visitor found for id: 1", result);
    }

    [TestMethod]
    public void GetById_ReturnsWorker()
    {
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);
        var worker = _workerService.GetById(_parkAdmin.Id);

        Assert.AreEqual(_parkAdmin.Id, worker!.Id);
    }

    [TestMethod]
    public void GetAll_ReturnsAllWorkers()
    {
        IQueryable<Worker> workers = new List<Worker> { _parkAdmin }.AsQueryable();

        _repository.Setup(r => r.GetAll()).Returns(workers);

        var result = _workerService.GetAll(null);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetAllWithEmail_ReturnsWorker()
    {
        IQueryable<Worker> workers = new List<Worker> { _parkAdmin }.AsQueryable();

        _repository.Setup(r => r.GetByEmail(_parkAdmin.Email)).Returns(_parkAdmin);

        var result = _workerService.GetAll(_parkAdmin.Email);

        Assert.AreEqual(1, result.Count());
    }

    [TestMethod]
    public void GetAllWithNonTrackedEmail_ReturnsEmptyList()
    {
        IQueryable<Worker> workers = new List<Worker> { _parkAdmin }.AsQueryable();

        _repository.Setup(r => r.GetByEmail(_parkAdmin.Email)).Returns(_nullWorker);

        var result = _workerService.GetAll(_parkAdmin.Email);

        Assert.AreEqual(0, result.Count());
    }

    [TestMethod]
    public void UpdateWorker_Ok()
    {
        _repository.Setup(r => r.Update(It.IsAny<Worker>(), false));

        _workerService.Update(_parkAdmin);
        _repository.VerifyAll();
    }

    [TestMethod]
    public void UpdateWorkerWithNewPassword_Ok()
    {
        _repository.Setup(r => r.Update(It.IsAny<Worker>(), true));
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);
        _workerService.Update(_parkAdmin, "1234ASDe@");
        _repository.VerifyAll();
    }

    [TestMethod]
    public void UpdateWorkerWithNewWrongPassword_ThrowsInvalidOperationException()
    {
        _repository.Setup(r => r.Update(It.IsAny<Worker>(), false));
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);
        Assert.ThrowsException<InvalidOperationException>(() => _workerService.Update(_parkAdmin, "Juanson"));
    }

    [TestMethod]
    public void UpdateWorkerInvalidId_ThrowArgumentNullException()
    {
        _repository.Setup(r => r.Update(It.IsAny<Worker>(), false));
        _parkAdmin.Id = 0;
        Assert.ThrowsException<ArgumentNullException>(() => _workerService.Update(_parkAdmin));
    }

    [TestMethod]
    public void AddRole_AddCorrectly()
    {
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);

        _workerService.AddRole(_parkAdmin.Id, nameof(ValidRoles.Operator));

        var result = _parkAdmin.Roles.Contains(nameof(ValidRoles.Operator)) && _parkAdmin.Roles.Contains(nameof(ValidRoles.ParkAdmin));

        Assert.IsTrue(result);
    }

    [TestMethod]
    public void RemoveRole_RemoveCorrectly()
    {
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);

        _workerService.RemoveRole(_parkAdmin.Id, nameof(ValidRoles.ParkAdmin));

        var result = _parkAdmin.Roles.Contains(nameof(ValidRoles.ParkAdmin));

        Assert.IsFalse(result);
    }

    [TestMethod]
    public void UpdateRole_EmptyRole_ThrowInvalidOperationException()
    {
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);

        _parkAdmin.Roles = string.Empty;

        Assert.ThrowsException<InvalidOperationException>(() => _workerService.Update(_parkAdmin));
    }

    [TestMethod]
    public void UpdateRole_WrongRole_ThrowInvalidOperationException()
    {
        _repository.Setup(r => r.GetById(_parkAdmin.Id)).Returns(_parkAdmin);

        _parkAdmin.Roles += "Asd,";

        Assert.ThrowsException<InvalidOperationException>(() => _workerService.Update(_parkAdmin));
    }

    [TestMethod]
    public void GetByEmail_GetCorrectly()
    {
        _repository.Setup(r => r.GetByEmail(_parkAdmin.Email)).Returns(_parkAdmin);

        var result = _workerService.GetByEmail(_parkAdmin.Email);

        Assert.AreEqual(_parkAdmin.Email, result!.Email);
    }
}
