namespace Domain;

public class Worker : User
{
    private string _roles;
    public Worker(string name, string password, string email)
      : base(name, password, email)
    {
        _roles = string.Empty;
    }

    public Worker()
    {
        _roles = string.Empty;
    }

    public string Roles
    {
        get => _roles;
        set => _roles = value;
    }
}
