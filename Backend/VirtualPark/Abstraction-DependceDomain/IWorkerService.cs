using Domain;
using NonDomainAbstractions;

namespace Abstraction;

public interface IWorkerService : ICRUDUserService<Worker>
{
    public Worker? GetByEmail(string email);
}
