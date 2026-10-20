using Exceptions;

namespace Tests.ExceptionsTests;

[TestClass]
public class MinimumAgeExceptionTest
{
    [TestMethod]
    public void MinimumAgeException_IsNotNull()
    {
        var exception = new MinimumAgeException();

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void MessageConstructor_SetMessage()
    {
        var exception = new MinimumAgeException("test");

        Assert.AreEqual("test", exception.Message);
    }
}
