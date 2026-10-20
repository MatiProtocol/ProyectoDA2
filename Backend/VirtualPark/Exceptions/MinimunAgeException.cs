namespace Exceptions;

public class MinimumAgeException(string? message = null) : Exception(message ?? string.Empty)
{
}
