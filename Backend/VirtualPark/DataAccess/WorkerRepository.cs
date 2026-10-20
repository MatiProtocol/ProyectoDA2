using Abstraction;
using Domain;

namespace DataAccess;

public class WorkerRepository(SqlContext context) : Repository<Worker>(context), IWorkerRepository
{
    public Worker? GetByEmail(string email)
    {
        return DbSet.FirstOrDefault(w => w.Email == email);
    }

    public void Update(Worker entity, bool changePassword = false)
    {
        var existing = Context.Workers.Find(entity.Id);
        existing!.Name = entity.Name;
        existing.Email = entity.Email;
        existing.Roles = entity.Roles;

        if(changePassword)
        {
            existing.Password = entity.Password;
        }

        Context.SaveChanges();
    }
}
