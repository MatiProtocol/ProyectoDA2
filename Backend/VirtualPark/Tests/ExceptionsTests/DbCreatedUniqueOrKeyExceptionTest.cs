using Exceptions;

namespace Tests.ExceptionsTests;

[TestClass]
public class DbCreatedUniqueOrKeyExceptionTest
{
    [TestMethod]
    public void DbCreatedUniqueOrKeyException_IsNotNull()
    {
        var exception = new DbCreatedUniqueOrKeyException();

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void MessageConstructor_SetMessage()
    {
        var exception = new DbCreatedUniqueOrKeyException("test");

        Assert.AreEqual("test", exception.Message);
    }
}
