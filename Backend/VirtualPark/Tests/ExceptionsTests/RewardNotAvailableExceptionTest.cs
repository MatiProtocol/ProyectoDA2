using Exceptions;

namespace Tests.ExceptionsTests;

[TestClass]
public class RewardNotAvailableExceptionTest
{
    [TestMethod]
    public void RewardNotAvailableException_IsNotNull()
    {
        var exception = new RewardNotAvailableException();

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void DefaultConstructor_SetsDefaultMessage()
    {
        var exception = new RewardNotAvailableException();

        Assert.AreEqual("Reward is not available", exception.Message);
    }

    [TestMethod]
    public void MessageConstructor_SetsCustomMessage()
    {
        var exception = new RewardNotAvailableException("Custom message");

        Assert.AreEqual("Custom message", exception.Message);
    }
}
