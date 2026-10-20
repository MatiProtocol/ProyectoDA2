using Domain;
using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetWorkerResponseTest
{
    private Worker _worker = null!;
    private GetWorkerResponse _getWorkerResponse = null!;

    [TestInitialize]
    public void Initialize()
    {
        _worker = new Worker
        {
            Name = "Mati",
            Id = 0,
            Email = "mati@tima.com"
        };
        _getWorkerResponse = new GetWorkerResponse(_worker);
    }

    [TestMethod]
    public void CreateWorkerDto_NotNull()
    {
        Assert.IsNotNull(_getWorkerResponse);
    }

    [TestMethod]
    public void SetWorkerDtoIdGet_ReturnCorrectly()
    {
        _worker.Id = 1;
        _getWorkerResponse = new GetWorkerResponse(_worker);
        _getWorkerResponse.Id = 1;
        Assert.AreEqual(_worker.Id, _getWorkerResponse.Id);
    }

    [TestMethod]
    public void SetWorkerDtoNameGetCorrectly()
    {
        _worker.Name = "Pedro";
        _getWorkerResponse = new GetWorkerResponse(_worker);
        _getWorkerResponse.Name = "Pedro";
        Assert.AreEqual(_worker.Name, _getWorkerResponse.Name);
    }

    [TestMethod]
    public void SetWorkerDtoEmailGetCorrectly()
    {
        _worker.Email = "calle@trece.com";
        _getWorkerResponse = new GetWorkerResponse(_worker);
        _getWorkerResponse.Email = "calle@trece.com";
        Assert.AreEqual(_worker.Email.ToString(), _getWorkerResponse.Email);
    }

    [TestMethod]
    public void SetWorkerType_GetCorrectly()
    {
        _getWorkerResponse.Roles = "Operator";
        Assert.AreEqual(_getWorkerResponse.Roles, "Operator");
    }
}
