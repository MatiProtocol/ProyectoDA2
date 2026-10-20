namespace Exceptions;

public class RewardNotAvailableException(string message = "Reward is not available")
    : Exception(message)
{
}
