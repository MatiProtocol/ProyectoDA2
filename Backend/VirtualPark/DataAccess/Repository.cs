using Microsoft.EntityFrameworkCore;
using NonDomainAbstractions;

namespace DataAccess;

public class Repository<T>(SqlContext context) : IRepository<T>
    where T : class
{
    private protected SqlContext Context { get; } = context;
    private protected DbSet<T> DbSet { get; } = context.Set<T>();

    public void Add(T entity)
    {
        DbSet.Add(entity);
        Context.SaveChanges();
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
        Context.SaveChanges();
    }

    public virtual void Update(T entity)
    {
        DbSet.Update(entity);
        Context.SaveChanges();
    }

    public virtual T? GetById(int id) => DbSet.Find(id);

    public IQueryable<T> Query() => DbSet.AsQueryable();

    public IEnumerable<T> GetAll() => DbSet.AsQueryable();
}
