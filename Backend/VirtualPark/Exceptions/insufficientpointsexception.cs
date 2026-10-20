namespace Exceptions;

public class InsufficientPointsException(int requiredPoints, int availablePoints)
    : Exception($"Insufficient points. Required: {requiredPoints}, Available: {availablePoints}")
{
    public int RequiredPoints { get; } = requiredPoints;
    public int AvailablePoints { get; } = availablePoints;
}
