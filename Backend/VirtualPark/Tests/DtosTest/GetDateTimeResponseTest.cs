using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class GetDateTimeResponseTest
{
    [TestMethod]
    public void GetDateTimeResponse_IsNotNull()
    {
        var getDateTimResponse = new GetDateTimeResponse
        {
            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        Assert.IsNotNull(getDateTimResponse);
    }

    [TestMethod]
    public void SetDateTime_GetCorrectly()
    {
        var getDateTimResponse = new GetDateTimeResponse
        {
            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
        };

        var result = getDateTimResponse.DateTime;

        Assert.AreEqual(result, DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
    }
}
