namespace NonDomainAbstractions;

public interface IRUService<T>
    where T : class
{
    T? GetById(int id);
    IEnumerable<T> GetAll();
    void Update(T entity);
}
