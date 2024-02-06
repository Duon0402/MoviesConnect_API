using System.Security.Claims;

namespace API.Extentions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
        {
            var roleClaims = user.FindAll(ClaimTypes.Role);
            return roleClaims.Select(c => c.Value);
        }
    }
}
