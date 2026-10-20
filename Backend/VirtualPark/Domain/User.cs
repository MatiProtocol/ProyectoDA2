namespace Domain;

public abstract class User
{
    private int _id;
    private string _name;
    private string _password;
    private string _email;

    protected User(string name, string password, string email)
    {
        _name = name;
        _password = password;
        _email = email;
    }

    protected User()
    {
        _name = string.Empty;
        _password = string.Empty;
        _email = "default@dummy";
    }

    public string Name
    {
        get => _name;
        set => _name = value;
    }

    public string Password
    {
        get => _password;
        set => _password = value;
    }

    public string Email
    {
        get => _email;
        set => _email = value;
    }

    public int Id
    {
        get => _id;
        set => _id = value;
    }
}
