using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VirtualParkApi.Filters;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class OwnAccountOrRoleFilter(string[] roles, bool allRoles = false, string routeParameter = "id")
    : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var ownDataFilter = new OwnDataAccountFilter(routeParameter);
        var roleFilter = new RolePermissionFilter(roles, allRoles);

        ownDataFilter.OnAuthorization(context);

        if(context.Result == null)
        {
            return;
        }

        context.Result = null;
        roleFilter.OnAuthorization(context);
    }
}
