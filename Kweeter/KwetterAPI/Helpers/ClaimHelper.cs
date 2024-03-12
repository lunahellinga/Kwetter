using System.Security.Claims;

namespace KwetterAPI.Helpers;

public static class ClaimHelper
{
    public static Guid GetUserId(ClaimsPrincipal claimsPrincipal)
    {
        return new Guid(claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        throw new InvalidOperationException());
    }

    public static string GetUserName(ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.Identity.Name ?? throw new InvalidOperationException();
    }
}