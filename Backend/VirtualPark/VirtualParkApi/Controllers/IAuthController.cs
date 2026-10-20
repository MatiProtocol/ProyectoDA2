using Microsoft.AspNetCore.Mvc;
using VirtualParkApi.Request;

namespace VirtualParkApi.Controllers;

public interface IAuthController
{
    IActionResult VisitorLogin(LoginRequest request);
    IActionResult WorkerLogin(LoginRequest request);
}
