using Exceptions;

namespace Tests.ExceptionsTests;

[TestClass]
public class DbKeyDoesNotExistExceptionTest
{
    [TestMethod]
    public void DbKeyDoesNotExistExceptionTest_IsNotNull()
    {
        var exception = new DbKeyDoesNotExistException();

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void MessageConstructor_SetMessage()
    {
        var exception = new DbKeyDoesNotExistException("test");

        Assert.AreEqual("test", exception.Message);
    }
}
