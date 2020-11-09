using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CoursesMain
{
    public static class IdentityHelper
    {
        public static string GetProfilePicture(this IIdentity identity)
        {
            var claimIdent = identity as ClaimsIdentity;
            return claimIdent != null
                && claimIdent.HasClaim(c => c.Type == "ProfilePicture")
                ? claimIdent.FindFirst("ProfilePicture").Value
                : string.Empty;
        }
    }
}
