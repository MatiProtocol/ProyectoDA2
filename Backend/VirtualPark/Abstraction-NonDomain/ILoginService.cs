namespace NonDomainAbstractions;

public interface ILoginService
{
    public string LoginWorker(string email, string password);
    public string LoginVisitor(string email, string password);
}
