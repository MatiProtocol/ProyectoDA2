using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class SetDateTimeRequestTest
{
    [TestMethod]
    public void SetDateTimeRequest_IsNotNull()
    {
        var setDateTimeRequest = new SetDateTimeRequest(DateTime.Now);

        Assert.IsNotNull(setDateTimeRequest);
    }

    [TestMethod]
    public void SetDate_GetCorrectly()
    {
        var setDateTimeRequest = new SetDateTimeRequest(DateTime.Now.Date);

        Assert.AreEqual(setDateTimeRequest.DateTime, DateTime.Now.Date.AddHours(3));
    }
}
