namespace NonDomainAbstractions;

public interface IRUUserService<T>
where T : class
{
    T? GetById(int id);
    IEnumerable<T> GetAll(string? email = null);
    void Update(T entity, string? oldPassword = null);
}
