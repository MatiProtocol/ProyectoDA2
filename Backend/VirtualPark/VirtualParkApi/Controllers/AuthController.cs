using Microsoft.AspNetCore.Mvc;
using NonDomainAbstractions;
using VirtualParkApi.Request;
using VirtualParkApi.Response;

namespace VirtualParkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ILoginService loginService) : Controller, IAuthController
{
    private readonly ILoginService _loginService = loginService;

    [HttpPost("Visitor/Login")]
    public IActionResult VisitorLogin(LoginRequest request)
    {
        var token = _loginService.LoginVisitor(request.Email, request.Password);
        return Ok(new LoginResponse { Token = token });
    }

    [HttpPost("Worker/Login")]
    public IActionResult WorkerLogin(LoginRequest request)
    {
        var token = _loginService.LoginWorker(request.Email, request.Password);
        return Ok(new LoginResponse { Token = token });
    }
}
