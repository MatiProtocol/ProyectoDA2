namespace NonDomainAbstractions;

public interface ICRUDUserService<T> : IRUUserService<T>
where T : class
{
    void Create(T entity);
    void Delete(int id);
}
