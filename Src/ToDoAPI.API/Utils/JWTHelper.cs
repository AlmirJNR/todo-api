using System.Security.Claims;
using Contracts.Interfaces.Services;
using Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Utils;

public abstract class JWTHelper
{
    public static async Task<(User?, IActionResult?)> UserExists(
        IUserService userService,
        ClaimsPrincipal claimsPrincipal)
    {
        var (claimValue, actionResult) = ClaimExists(claimsPrincipal, "userId");
        if (actionResult is not null)
            return (null, actionResult);

        var guidWasParsed = Guid.TryParse(claimValue, out var userAuthServerGuid);
        if (!guidWasParsed)
            return (null, new BadRequestResult());

        var user = await userService.GetUserById(userAuthServerGuid);
        if (user is null)
            return (null, new NotFoundResult());

        return (user, null);
    }

    public static (string? claimValue, IActionResult?) ClaimExists(ClaimsPrincipal claimsPrincipal, string claimToCheck)
    {
        var claimDictionary = claimsPrincipal.Claims.ToDictionary(claim => claim.Type, claim => claim.Value);
        var claimExists = claimDictionary.TryGetValue(claimToCheck, out var claimValue);
        if (!claimExists || string.IsNullOrWhiteSpace(claimValue))
            return (null, new BadRequestResult());

        return (claimValue, null);
    }
}