namespace Exceptions;

public class ResourceNotFoundException(string resourceName, int id)
    : Exception($"{resourceName} with id {id} not found")
{
    public string ResourceName { get; } = resourceName;
    public int ResourceId { get; } = id;
}
