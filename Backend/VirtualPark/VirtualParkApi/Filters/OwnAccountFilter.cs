using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VirtualParkApi.Filters;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class OwnDataAccountFilter(string routeParameter = "id") : Attribute, IAuthorizationFilter
{
    private readonly string _id = routeParameter;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)
                          ?? context.HttpContext.User.FindFirst("sub");
        var userTypeClaim = context.HttpContext.User.FindFirst("userType");

        if(userIdClaim == null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var path = context.HttpContext.Request.Path.Value?.ToLower() ?? string.Empty;

        if(path.Contains("visitor") && userTypeClaim != null && userTypeClaim.Value != "Visitor")
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        if(!context.RouteData.Values.TryGetValue(_id, out var routeId) ||
            routeId?.ToString() != userIdClaim.Value)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
