using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetCapacityResponseTest
{
    private GetCapacityResponse _response = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _response = new GetCapacityResponse
        {
            CurrentCapacity = 1,
            RemainingCapacity = 1
        };
    }

    [TestMethod]
    public void GetCapacityResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void SetCurrentCapacity_GetCorrectly()
    {
        Assert.AreEqual(1, _response.CurrentCapacity);
    }

    [TestMethod]
    public void SetRemainingCapacity_GetCorrectly()
    {
        Assert.AreEqual(1, _response.RemainingCapacity);
    }
}
