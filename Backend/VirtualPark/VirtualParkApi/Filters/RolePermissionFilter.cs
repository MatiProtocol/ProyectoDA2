using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VirtualParkApi.Filters;
[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class RolePermissionFilter(string[] roles, bool allRoles = false) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userRole = context.HttpContext.User.FindFirst(ClaimTypes.Role);
        bool authorized;
        if(allRoles)
        {
            authorized = VerifyAllPermission(roles, userRole);
        }
        else
        {
            authorized = VerifyOnePermission(roles, userRole);
        }

        if(!authorized)
        {
            context.Result = new UnauthorizedResult();
        }
    }

    private bool VerifyAllPermission(string[] requiredRoles, Claim? userRole)
    {
        if(userRole == null)
        {
            return false;
        }

        var isCorrect = false;
        foreach(var role in requiredRoles)
        {
            if(userRole.Value.Contains(role))
            {
                isCorrect = true;
            }
            else
            {
                isCorrect = false;
                break;
            }
        }

        return isCorrect;
    }

    private bool VerifyOnePermission(string[] requiredRoles, Claim? userRole)
    {
        if(userRole == null)
        {
            return false;
        }

        foreach(var role in requiredRoles)
        {
            if(userRole.Value.Contains(role))
            {
                return true;
            }
        }

        return false;
    }
}
