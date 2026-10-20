using Abstraction;
using Domain;
using Exceptions;

namespace Services;

public class WorkerService(IWorkerRepository repository) : IWorkerService
{
    private readonly IWorkerRepository _repository = repository;

    public Worker? GetById(int id)
    {
        return _repository.GetById(id);
    }

    public IEnumerable<Worker> GetAll(string? email = null)
    {
        if(email == null)
        {
            return _repository.GetAll();
        }

        var response = _repository.GetByEmail(email);
        return response != null
            ? new List<Worker> { response }.AsEnumerable()
            : new List<Worker>().AsEnumerable();
    }

    public void Create(Worker entity)
    {
        if(_repository.GetByEmail(entity.Email) != null)
        {
            throw new DbCreatedUniqueOrKeyException("The email already exists");
        }

        if(Enum.TryParse<ValidRoles>(entity.Roles, out var role))
        {
            entity.Roles = role.ToString() + ',';
            _repository.Add(entity);
        }
        else
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException("Invalid role", "Role"));
        }
    }

    private bool IsValidRole(Worker entity)
    {
        var roles = entity.Roles.Split(',');

        if(roles.Length == 1 || !entity.Roles.EndsWith(','))
        {
            return false;
        }

        foreach(var role in roles)
        {
            if(!Enum.TryParse<ValidRoles>(role, out _) && role != string.Empty)
            {
                return false;
            }
        }

        return true;
    }

    public void Delete(int id)
    {
        var worker = _repository.GetById(id);
        if(worker != null)
        {
            _repository.Remove(worker);
        }
        else
        {
            throw new Exception($"No visitor found for id: {id}");
        }
    }

    public Worker? GetByEmail(string email)
    {
        return _repository.GetByEmail(email);
    }

    public void AddRole(int id, string role)
    {
        var worker = GetById(id);
        var roles = worker!.Roles;

        if(Enum.TryParse<ValidRoles>(role, out var _))
        {
            if(!roles.Contains(role))
            {
                worker.Roles += role + ",";
            }
        }
    }

    public void RemoveRole(int id, string role)
    {
        var worker = GetById(id);
        var roles = worker!.Roles;

        if(Enum.TryParse<ValidRoles>(role, out var _))
        {
            var index = roles.IndexOf(role, StringComparison.Ordinal);
            if(index >= 0)
            {
                worker.Roles = worker.Roles.Remove(index, role.Length + 1);
            }
        }
    }

    public void Update(Worker entity, string? oldPassword = null)
    {
        if(entity.Id <= 0)
        {
            throw new ArgumentNullException($"No worker found with id {entity.Id}", new DbKeyDoesNotExistException());
        }

        var isValid = IsValidRole(entity);

        if(!isValid)
        {
            throw new InvalidOperationException(string.Empty, new ArgumentException("Invalid role", "Role"));
        }

        var changePassword = false;
        if(oldPassword != null)
        {
            var worker = GetById(entity.Id);
            changePassword = worker!.Password == oldPassword;
            if(!changePassword)
            {
                throw new InvalidOperationException(string.Empty, new ArgumentException("Password does not match with user password", "Password"));
            }
        }

        _repository.Update(entity, changePassword);
    }
}
