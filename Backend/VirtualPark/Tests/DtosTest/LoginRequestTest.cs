using VirtualParkApi.Request;

namespace Tests.DtosTest;

[TestClass]
public class LoginRequestTest
{
    private LoginRequest _loginRequest = null!;

    [TestInitialize]
    public void Initialize()
    {
        _loginRequest = new LoginRequest
        {
            Email = "xD",
            Password = "123eaSDA"
        };
    }

    [TestMethod]
    public void CreateLoginRequest()
    {
        Assert.IsNotNull(_loginRequest);
    }

    [TestMethod]
    public void SetEmail_GetCorrectly()
    {
        const string CONST_EXAMPLE_EMAIL = "pepito@pepe.com";

        _loginRequest.Email = CONST_EXAMPLE_EMAIL;

        Assert.AreEqual(CONST_EXAMPLE_EMAIL, _loginRequest.Email);
    }

    [TestMethod]
    public void SetPassword_GetCorrectly()
    {
        const string CONST_EXAMPLE_PASSWORD = "123456";

        _loginRequest.Password = CONST_EXAMPLE_PASSWORD;

        Assert.AreEqual(CONST_EXAMPLE_PASSWORD, _loginRequest.Password);
    }
}
