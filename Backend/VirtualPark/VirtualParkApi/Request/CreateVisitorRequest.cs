using Domain;

namespace VirtualParkApi.Request;

public class CreateVisitorRequest
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required DateTime Birthdate { get; set; }
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }

    public Visitor ToEntity(int id = 0)
    {
        return new Visitor
        {
            Name = Name,
            Email = Email,
            BirthDate = Birthdate,
            Id = id,
            Password = NewPassword ?? string.Empty
        };
    }
}
