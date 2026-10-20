namespace Exceptions;

public class MembershipLevelRequirementException(string requiredLevel, string currentLevel)
    : Exception($"Membership level requirement not met. Required: {requiredLevel}, Current: {currentLevel}")
{
    public string RequiredLevel { get; } = requiredLevel;
    public string CurrentLevel { get; } = currentLevel;
}
