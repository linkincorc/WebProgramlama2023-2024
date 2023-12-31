using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using PlaneTicket.Authorization;
using PlaneTicket.Data;
using PlaneTicket.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    // Global authorization policy - requires authenticated user for all endpointsD:\WebApps\.NET\PlaneTDirectoryNotFoundException: 'D:\WebApps\.NET\PlaneTicket\wwwrooticket\wwwroot
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });
builder.Services.AddSingleton<IAuthorizationHandler, AdminOrRedirectHandler>();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrRedirect", policy =>
        policy.Requirements.Add(new AdminOrRedirectRequirement()));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedData.Initialize(services);
}

app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint();
    if (endpoint?.Metadata?.GetMetadata<IAuthorizeData>() != null)
    {
        var authService = context.RequestServices.GetRequiredService<IAuthorizationService>();
        var result = await authService.AuthorizeAsync(context.User, "AdminOrRedirect");
        if (!result.Succeeded)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                // Trigger the default challenge (redirect to login)
                await context.ChallengeAsync();
                return;
            }
            else
            {
                // Redirect to Flights/Index if the user is authenticated but not an admin
                context.Response.Redirect("/Flights/Index");
                return;
            }
        }
    }

    await next();
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();