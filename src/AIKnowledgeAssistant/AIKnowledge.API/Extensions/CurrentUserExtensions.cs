using System.Security.Claims;

namespace AIKnowledge.API.Extensions;

public static class CurrentUserExtensions
{
    public static int GetUserId(this ClaimsPrincipal user)
    {
        var value = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        return value == null ? 0 : Convert.ToInt32(value);
    }

    public static string GetEmail(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Email)?.Value ?? "";
    }

    public static string GetRole(this ClaimsPrincipal user)
    {
        return user.FindFirst(ClaimTypes.Role)?.Value ?? "";
    }
}

