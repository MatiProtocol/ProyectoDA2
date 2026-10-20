using DataAccess;
using Domain;

namespace Tests.RepositoriesTest;

[TestClass]
public class WorkerRepositoryTest
{
    private WorkerRepository _workerRepository = null!;
    private SqlContext _context = null!;
    private Worker _worker = null!;

    [TestInitialize]
    public void Initialize()
    {
        _context = SqlContextInMemoryCreate.Create();

        _worker = new Worker("pepe", "asdasd@12", "asda@asd.com")
        {
            Roles = "ParkAdmin"
        };

        _context.Workers.Add(_worker);
        _context.SaveChanges();

        _workerRepository = new WorkerRepository(_context);
    }

    [TestMethod]
    public void WorkerRepository_IsNotNull()
    {
        Assert.IsNotNull(_workerRepository);
    }

    [TestMethod]
    public void GetByEmail_IsNotNull()
    {
        var result = _workerRepository.GetByEmail("asda@asd.com");
        Assert.IsNotNull(result);
        Assert.AreEqual(_worker.Id, result!.Id);
    }

    [TestMethod]
    public void UpdateWorker_IsNotNull()
    {
        _worker.Name = "Paco";
        _workerRepository.Update(_worker);
        Assert.AreEqual(_worker.Name, _workerRepository.GetById(_worker.Id)!.Name);
    }

    [TestMethod]
    public void UpdateWorkerChangePassword_IsNotNull()
    {
        _worker.Name = "Paco";
        _workerRepository.Update(_worker, true);
        Assert.AreEqual(_worker.Name, _workerRepository.GetById(_worker.Id)!.Name);
    }
}
