using Domain;

namespace Tests.DomainTest;

[TestClass]
public class WorkerTest
{
    [TestMethod]
    public void Worker_IsNotNull()
    {
        var result = new Worker();

        Assert.IsNotNull(result);
    }

    [TestMethod]
    public void SetRoles_GetCorrectly()
    {
        var worker = new Worker("pepe", "asdasd@12", "asda@asd.com");
        worker.Roles = "ParkAdmin";

        var result = worker.Roles;

        Assert.AreEqual("ParkAdmin", result);
    }
}
