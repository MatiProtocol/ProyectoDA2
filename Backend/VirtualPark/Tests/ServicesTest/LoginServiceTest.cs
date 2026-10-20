using Abstraction;
using Domain;
using Microsoft.Extensions.Configuration;
using Moq;
using Services;

namespace Tests.ServicesTest;

[TestClass]
public class LoginServiceTest
{
    private Mock<IWorkerService> _workerService = null!;
    private Mock<IVisitorService> _visitorService = null!;
    private Dictionary<string, string> _settings = null!;
    private LoginService _loginService = null!;
    private readonly Worker _nullWorker = null!;
    private readonly Visitor _nullVisitor = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _workerService = new Mock<IWorkerService>(MockBehavior.Strict);
        _visitorService = new Mock<IVisitorService>(MockBehavior.Strict);
        _settings = new Dictionary<string, string>
        {
            { "Jwt:Key", "MI_CLAVE_SUPER_SECRETA_DE_32_BYTES_1234567890" },
            { "Jwt:Issuer", "VirtualPark" },
            { "Jwt:Audience", "VirtualPark" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_settings!)
            .Build();

        _loginService = new LoginService(_workerService.Object, _visitorService.Object, configuration);
    }

    [TestMethod]
    public void LoginService_IsNotNull()
    {
        Assert.IsNotNull(_loginService);
    }

    [TestMethod]
    public void GenerateToken_IsNotNull()
    {
        var worker = new Worker
        {
            Email = "Example@Example.com",
            Name = "Example",
            Password = "asda@asdas!1",
            Roles = "Operator,"
        };

        var result = _loginService.GenerateToken(worker);

        Assert.IsNotNull(string.Empty, result);
    }

    [TestMethod]
    public void LoginWorker_IsNotNull()
    {
        var worker = new Worker
        {
            Email = "Example@Example.com",
            Name = "Example",
            Password = "asda@asdas!1",
            Roles = "Operator,"
        };

        _workerService.Setup(v => v.GetByEmail(worker.Email)).Returns(worker);

        var result = _loginService.LoginWorker(worker.Email, worker.Password);

        Assert.IsNotNull(string.Empty, result);
    }

    [TestMethod]
    public void LoginWorker_NonTrackedWorker_ThrowArgumentException_WithInnerUnauthorizeException()
    {
        var worker = new Worker
        {
            Email = "Example@Example.com",
            Name = "Example",
            Password = "asda@asdas!1",
            Roles = "Operator,"
        };

        _workerService.Setup(v => v.GetByEmail(worker.Email)).Returns(_nullWorker);

        ArgumentException result = null!;
        try
        {
            _loginService.LoginWorker(worker.Email, worker.Password);
        }
        catch(Exception ex)
        {
            result = (ArgumentException)ex;
        }

        Assert.IsInstanceOfType<UnauthorizedAccessException>(result.InnerException);
    }

    [TestMethod]
    public void LoginVisitor_IsNotNull()
    {
        var visitor = new Visitor
        {
            Email = "Example@Example.com",
            Name = "Example",
            Password = "asda@asdas!1",
            BirthDate = DateTime.Now
        };

        _visitorService.Setup(v => v.GetByEmail(visitor.Email)).Returns(visitor);

        var result = _loginService.LoginVisitor(visitor.Email, visitor.Password);

        Assert.IsNotNull(string.Empty, result);
    }

    [TestMethod]
    public void LoginVisitor_NonTrackedWorker_ThrowArgumentException_WithInnerUnauthorizeException()
    {
        var visitor = new Visitor
        {
            Email = "Example@Example.com",
            Name = "Example",
            Password = "asda@asdas!1",
            BirthDate = DateTime.Now
        };

        _visitorService.Setup(v => v.GetByEmail(visitor.Email)).Returns(_nullVisitor);

        ArgumentException result = null!;
        try
        {
            _loginService.LoginVisitor(visitor.Email, visitor.Password);
        }
        catch(Exception ex)
        {
            result = (ArgumentException)ex;
        }

        Assert.IsInstanceOfType<UnauthorizedAccessException>(result.InnerException);
    }

    [TestMethod]
    public void LoginVisitor_WrongPassword_ThrowUnauthorizedException()
    {
        const string CONST_WRONG_PASSWORD = "wrongPassword";
        var visitor = new Visitor
        {
            Email = "Example@Example.com",
            Name = "Example",
            Password = "asda@asdas!1",
            BirthDate = DateTime.Now
        };

        _visitorService.Setup(v => v.GetByEmail(visitor.Email)).Returns(visitor);

        ArgumentException result = null!;
        try
        {
            _loginService.LoginVisitor(visitor.Email, CONST_WRONG_PASSWORD);
        }
        catch(Exception ex)
        {
            result = (ArgumentException)ex;
        }

        Assert.IsInstanceOfType<UnauthorizedAccessException>(result.InnerException);
    }

    [TestMethod]
    public void LoginWorker_WrongEmail_ThrowUnauthorizedException()
    {
        const string CONST_WRONG_PASSWORD = "wrongPassword";

        var worker = new Worker
        {
            Email = "Example@Example.com",
            Name = "Example",
            Password = "asda@asdas!1",
            Roles = "Operator,"
        };

        _workerService.Setup(v => v.GetByEmail(worker.Email)).Returns(worker);

        ArgumentException result = null!;
        try
        {
            _loginService.LoginWorker(worker.Email, CONST_WRONG_PASSWORD);
        }
        catch(Exception ex)
        {
            result = (ArgumentException)ex;
        }

        Assert.IsInstanceOfType<UnauthorizedAccessException>(result.InnerException);
    }
}
