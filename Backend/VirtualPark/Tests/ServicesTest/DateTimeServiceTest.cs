using Services;

namespace Tests.ServicesTest;

[TestClass]
public class DateTimeServiceTest
{
    [TestMethod]
    public void DateTimeService_IsNotNull()
    {
        var dateTimeService = new DateTimeService();

        Assert.IsNotNull(dateTimeService);
    }

    [TestMethod]
    public void GetDateTime_WithNonConfiguredDate_GetNow()
    {
        var dateTimeService = new DateTimeService();

        var now = DateTime.Now.AddHours(-3).Date;

        var result = dateTimeService.Now.Date;

        Assert.AreEqual(now, result);
    }

    [TestMethod]
    public void SetDateTime_GetCorrectly()
    {
        var dateTimeService = new DateTimeService();

        var now = DateTime.Parse("2025/09/28 00:00");

        dateTimeService.Now = now;

        var result = dateTimeService.Now;

        Assert.AreEqual(now.AddHours(-3), result);
    }
}
