namespace NonDomainAbstractions;

public interface ICrudService<T> : IRUService<T>
    where T : class
{
    void Create(T entity);
    void Delete(int id);
}
