using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Elevel.Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetLoggedInUserId(this ClaimsPrincipal principal)
        {
            var claims = principal.Claims.ToList();

            var userId = claims.FirstOrDefault(x => x.Type == "uid").Value;

            return Guid.Parse(userId);
        }
        public static string GetLoggedInUserRole(this ClaimsPrincipal principal)
        {
            var claims = principal.Claims.ToList();

            var userRole = claims.FirstOrDefault(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;

            return userRole;
        }
    }
}