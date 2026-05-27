using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Infrastructure;

public class RoleAuthorizeAttribute(string requiredRole) : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedObjectResult("Authentication required.");
            return;
        }

        var role = user.FindFirst(ClaimTypes.Role)?.Value;
        if (!string.Equals(role, requiredRole, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ObjectResult("Forbidden: insufficient role.") { StatusCode = 403 };
        }
    }
}
