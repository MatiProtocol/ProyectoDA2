using System.Diagnostics.CodeAnalysis;
using System.Text;
using Abstraction;
using DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NonDomainAbstractions;
using Services;
using VirtualParkApi.Filters;

namespace VirtualParkApi;

[ExcludeFromCodeCoverage]
public static class ProgramConfig
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SqlContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<SqlContext, SqlContext>();
        services.AddDbContext<DbContext, SqlContext>();

        return services;
    }

    public static IServiceCollection AddCustomControllers(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
        });

        return services;
    }

    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IVisitorRepository, VisitorRepository>();
        services.AddScoped<IWorkerRepository, WorkerRepository>();
        services.AddScoped<IAttractionRepository, AttractionRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<IIncidenceRepository, IncidenceRepository>();
        services.AddScoped<ISpecialEventRepository, SpecialEventRepository>();
        services.AddScoped<IPointsHistoryRepository, PointsHistoryRepository>();
        services.AddScoped<IRewardRepository, RewardRepository>();
        services.AddScoped<IRewardRedemptionRepository, RewardRedemptionRepository>();

        services.AddScoped<IPunctuationService, PunctuationService>();
        services.AddScoped<ISpecialEventService, SpecialEventService>();
        services.AddScoped<IVisitorService, VisitorService>();
        services.AddScoped<IWorkerService, WorkerService>();
        services.AddScoped<IAttractionService, AttractionService>();
        services.AddScoped<ITicketService, TicketService>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<IIncidenceService, IncidenceService>();
        services.AddScoped<IPointsHistoryService, PointsHistoryService>();
        services.AddScoped<IRewardService, RewardService>();
        return services;
    }

    public static IServiceCollection AddCustomCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder
                    .WithOrigins("http://localhost:8080", "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]
                                         ?? throw new InvalidOperationException("Jwt:Key not found"));

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

        return services;
    }
}
