using Exceptions;

namespace Tests.ExceptionsTests;

[TestClass]
public class MembershipLevelRequirementExceptionTest
{
    [TestMethod]
    public void MembershipLevelRequirementException_IsNotNull()
    {
        var exception = new MembershipLevelRequirementException("Premium", "Standard");

        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public void Constructor_SetsRequiredLevel()
    {
        var exception = new MembershipLevelRequirementException("Premium", "Standard");

        Assert.AreEqual("Premium", exception.RequiredLevel);
    }

    [TestMethod]
    public void Constructor_SetsCurrentLevel()
    {
        var exception = new MembershipLevelRequirementException("Premium", "Standard");

        Assert.AreEqual("Standard", exception.CurrentLevel);
    }

    [TestMethod]
    public void Constructor_SetsCorrectMessage()
    {
        var exception = new MembershipLevelRequirementException("VIP", "Basic");

        Assert.AreEqual("Membership level requirement not met. Required: VIP, Current: Basic", exception.Message);
    }
}
