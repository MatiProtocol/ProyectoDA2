namespace NonDomainAbstractions;

public interface IUserRepository<T> : IRepository<T>
where T : class
{
    void Update(T user, bool changePassword);
}
