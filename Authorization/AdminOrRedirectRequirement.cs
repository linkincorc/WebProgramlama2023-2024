using Microsoft.AspNetCore.Authorization;

namespace PlaneTicket.Authorization;

public class AdminOrRedirectRequirement : IAuthorizationRequirement
{
    // Custom requirement logic goes here
}