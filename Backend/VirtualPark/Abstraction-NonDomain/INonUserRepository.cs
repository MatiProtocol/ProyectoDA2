namespace NonDomainAbstractions;

public interface INonUserRepository<T> : IRepository<T>
    where T : class
{
    void Update(T entity);
}
