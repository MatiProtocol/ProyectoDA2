using Exceptions;

namespace Tests.ExceptionsTests;

[TestClass]
public class ResourceNotFoundExceptionTest
{
    [TestMethod]
    public void ResourceNotFoundException_IsNotNull()
    {
        var exception = new ResourceNotFoundException("Visitor", 1);

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void Constructor_SetsResourceName()
    {
        var exception = new ResourceNotFoundException("Visitor", 1);

        Assert.AreEqual("Visitor", exception.ResourceName);
    }

    [TestMethod]
    public void Constructor_SetsResourceId()
    {
        var exception = new ResourceNotFoundException("Reward", 5);

        Assert.AreEqual(5, exception.ResourceId);
    }

    [TestMethod]
    public void Constructor_SetsCorrectMessage()
    {
        var exception = new ResourceNotFoundException("Visitor", 10);

        Assert.AreEqual("Visitor with id 10 not found", exception.Message);
    }
}
