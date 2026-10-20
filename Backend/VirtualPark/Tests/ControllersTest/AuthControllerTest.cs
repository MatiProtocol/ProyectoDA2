using Microsoft.AspNetCore.Mvc;
using Moq;
using NonDomainAbstractions;
using VirtualParkApi.Controllers;
using VirtualParkApi.Request;

namespace Tests.ControllersTest;

[TestClass]
public class AuthControllerTest
{
    private Mock<ILoginService> _loginService = null!;

    private IAuthController _authController = null!;

    [TestInitialize]
    public void Initialize()
    {
        _loginService = new Mock<ILoginService>(MockBehavior.Strict);
        _authController = new AuthController(_loginService.Object);
    }

    [TestMethod]
    public void AuthController_IsNotNull()
    {
        Assert.IsNotNull(_authController);
    }

    [TestMethod]
    public void LoginVisitor_Ok()
    {
        const string CONST_TOKEN = "asdasdazxc";

        var request = new LoginRequest
        {
            Email = "asdas@asd.com",
            Password = "asdas123"
        };

        _loginService.Setup(l => l.LoginVisitor(request.Email, request.Password)).Returns(CONST_TOKEN);

        Assert.IsInstanceOfType<OkObjectResult>(_authController.VisitorLogin(request));
    }

    [TestMethod]
    public void LoginWorker_Ok()
    {
        const string CONST_TOKEN = "asdasdazxc";

        var request = new LoginRequest
        {
            Email = "asdas@asd.com",
            Password = "asdas123"
        };

        _loginService.Setup(l => l.LoginWorker(request.Email, request.Password)).Returns(CONST_TOKEN);

        Assert.IsInstanceOfType<OkObjectResult>(_authController.WorkerLogin(request));
    }
}
