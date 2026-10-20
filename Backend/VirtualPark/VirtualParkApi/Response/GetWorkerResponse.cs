using Domain;

namespace VirtualParkApi.Response;

public class GetWorkerResponse(Worker worker)
{
    public int Id { get; set; } = worker.Id;
    public string Name { get; set; } = worker.Name;
    public string Email { get; set; } = worker.Email.ToString();
    public string Roles { get; set; } = worker.Roles;
}
