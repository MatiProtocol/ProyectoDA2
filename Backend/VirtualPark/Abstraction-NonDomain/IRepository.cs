namespace NonDomainAbstractions;

public interface IRepository<T>
    where T : class
{
    void Add(T entity);
    void Remove(T entity);
    T? GetById(int id);
    IQueryable<T> Query();
    IEnumerable<T> GetAll();
}
