namespace Exceptions;

/// <summary>
/// This exception throws when a key or unique value already exists.
/// </summary>
public class DbCreatedUniqueOrKeyException(string? message = null) : Exception(message ?? string.Empty)
{
}
