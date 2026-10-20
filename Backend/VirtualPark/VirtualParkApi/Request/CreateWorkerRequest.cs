using Domain;

namespace VirtualParkApi.Request;

public class CreateWorkerRequest
{
    public required string Name { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public required string Email { get; set; }
    public required string Roles { get; set; }

    public Worker ToEntity(int id = 0)
    {
        return new Worker
        {
            Id = id,
            Name = Name,
            Password = NewPassword ?? string.Empty,
            Email = Email,
            Roles = Roles
        };
    }
}
