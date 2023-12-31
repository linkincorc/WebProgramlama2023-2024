using Microsoft.AspNetCore.Authorization;

namespace PlaneTicket.Authorization;

public class AdminOrRedirectHandler : AuthorizationHandler<AdminOrRedirectRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminOrRedirectRequirement requirement)
    {
        // Check if the user is authenticated
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Fail(); // This will trigger the default challenge (redirect to login)
            return Task.CompletedTask;
        }

        // Check if the user is an Admin
        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement); // User is an admin, proceed
        }
        else
        {
            context.Fail(); // Not an admin, access denied
        }

        return Task.CompletedTask;
    }
}