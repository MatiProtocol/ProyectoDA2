using Exceptions;

namespace Tests.ExceptionsTests;

[TestClass]
public class InsufficientPointsExceptionTest
{
    [TestMethod]
    public void InsufficientPointsException_IsNotNull()
    {
        var exception = new InsufficientPointsException(100, 50);

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void Constructor_SetsRequiredPoints()
    {
        var exception = new InsufficientPointsException(100, 50);

        Assert.AreEqual(100, exception.RequiredPoints);
    }

    [TestMethod]
    public void Constructor_SetsAvailablePoints()
    {
        var exception = new InsufficientPointsException(100, 50);

        Assert.AreEqual(50, exception.AvailablePoints);
    }

    [TestMethod]
    public void Constructor_SetsCorrectMessage()
    {
        var exception = new InsufficientPointsException(100, 30);

        Assert.AreEqual("Insufficient points. Required: 100, Available: 30", exception.Message);
    }
}
