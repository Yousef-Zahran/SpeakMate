using System.Security.Claims;

namespace SpeakMate.API.Extensions
{
    public static class ClaimsPrincipalExtension
    {

            public static string GetMemberId(this ClaimsPrincipal user)
            {
                return user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get memberId from token");
            }
        
    }
}
