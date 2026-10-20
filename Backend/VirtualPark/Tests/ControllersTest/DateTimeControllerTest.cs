using Microsoft.AspNetCore.Mvc;
using Services;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;

namespace Tests.ControllersTest;

[TestClass]
public class DateTimeControllerTest
{
    [TestMethod]
    public void DateTimeController_IsNotNull()
    {
        var dateTimeService = new DateTimeService();
        var dateTimeController = new DateTimeController(dateTimeService);

        Assert.IsNotNull(dateTimeController);
    }

    [TestMethod]
    public void SetDateTime_Ok()
    {
        var dateTimeService = new DateTimeService();
        var dateTimeController = new DateTimeController(dateTimeService);

        var request = new SetDateTimeRequest(DateTime.Parse("2002/02/18 00:00"));

        var result = dateTimeController.SetDateTime(request);

        Assert.IsInstanceOfType<OkResult>(result);
    }

    [TestMethod]
    public void GetDateTime_Ok()
    {
        var dateTimeService = new DateTimeService();
        var dateTimeController = new DateTimeController(dateTimeService);

        var result = dateTimeController.GetDateTime();

        Assert.IsInstanceOfType<OkObjectResult>(result);
    }
}
