using System;
using System.Security.Claims;
using System.Security.Principal;

namespace VerticalSliceArchitecture.API.Extensions
{
    public static class IIdentityExtensions
    {
        public static Guid GetId(this IIdentity identity)
        {
            var userId = ((ClaimsIdentity)identity).FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return Guid.Parse(userId);
        }
    }
}
