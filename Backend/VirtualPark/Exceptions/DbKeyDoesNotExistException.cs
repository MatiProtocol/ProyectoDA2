namespace Exceptions;

public class DbKeyDoesNotExistException(string? message = null) : Exception(message ?? string.Empty)
{
}
