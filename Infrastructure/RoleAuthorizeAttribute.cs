using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Infrastructure;

public class RoleAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _requiredRole;

    public RoleAuthorizeAttribute(string requiredRole)
    {
        _requiredRole = requiredRole;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var role = context.HttpContext.Request.Headers["X-User-Role"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(role))
        {
            context.Result = new UnauthorizedObjectResult("Missing X-User-Role header");
            return;
        }

        if (!string.Equals(role, _requiredRole, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new ForbidResult();
        }
    }
}