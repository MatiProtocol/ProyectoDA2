using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface ITicketController
{
    public IActionResult Create(BuyTicketRequest model);
    public IActionResult Remove(int id);
    public IActionResult GetById(int id);
    public IActionResult GetAll();
    public IActionResult GetTicketByQr(Guid qr);
}
