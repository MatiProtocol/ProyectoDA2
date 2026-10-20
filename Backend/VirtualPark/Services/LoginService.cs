using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Abstraction;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using NonDomainAbstractions;

namespace Services;

public class LoginService(IWorkerService ws, IVisitorService vs, IConfiguration config) : ILoginService
{
    private readonly IConfiguration _configuration = config;
    private readonly IWorkerService _workerService = ws;
    private readonly IVisitorService _visitorService = vs;

    public string LoginWorker(string email, string password)
    {
        var worker = _workerService.GetByEmail(email);
        if(worker is not null && worker.Password.Equals(password))
        {
            return GenerateToken(worker);
        }

        throw new ArgumentException("Wrong email or password", new UnauthorizedAccessException());
    }

    public string LoginVisitor(string email, string password)
    {
        var visitor = _visitorService.GetByEmail(email);
        if(visitor is not null && visitor.Password.Equals(password))
        {
            return GenerateToken(visitor);
        }

        throw new ArgumentException("Wrong email or password", new UnauthorizedAccessException());
    }

    public string GenerateToken(User user)
    {
        var keyString = _configuration["Jwt:Key"]
                        ?? throw new InvalidOperationException("Jwt:Key no configurado");
        var issuer = _configuration["Jwt:Issuer"]
                     ?? throw new InvalidOperationException("Jwt:Issuer no configurado");
        var audience = _configuration["Jwt:Audience"]
                       ?? throw new InvalidOperationException("Jwt:Audience no configurado");

        var key = Encoding.UTF8.GetBytes(keyString);
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("userType", user.GetType().Name)
        };

        if(user is Worker worker)
        {
            claims.Add(new Claim("role", worker.Roles));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
