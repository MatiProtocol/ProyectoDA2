using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IWorkerRepository : IUserRepository<Worker>
{
    public Worker? GetByEmail(string email);
}
