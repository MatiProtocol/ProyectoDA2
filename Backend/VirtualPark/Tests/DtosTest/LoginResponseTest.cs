using VirtualParkApi.Response;

namespace Tests.DtosTest;

[TestClass]
public class LoginResponseTest
{
    private LoginResponse _response = null!;

    [TestInitialize]
    public void Initialize()
    {
        _response = new LoginResponse
        {
            Token = "tokenExample",
        };
    }

    [TestMethod]
    public void LoginResponse_IsNotNull()
    {
        Assert.IsNotNull(_response);
    }

    [TestMethod]
    public void SetToken_GetCorrectly()
    {
        const string CONST_TOKEN = "token";
        var response = new LoginResponse
        {
            Token = CONST_TOKEN
        };

        Assert.AreEqual(CONST_TOKEN, response.Token);
    }
}
