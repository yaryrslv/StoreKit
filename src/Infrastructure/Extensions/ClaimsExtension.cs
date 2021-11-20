using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StoreKit.Infrastructure.Extensions
{
    public static class ClaimsExtension
    {
        public static async Task<IdentityResult> AddPermissionClaimAsync(this RoleManager<ApplicationRole> roleManager, ApplicationRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);
            if (!allClaims.Any(a => a.Type == ClaimConstants.Permission && a.Value == permission))
            {
                return await roleManager.AddClaimAsync(role, new Claim(ClaimConstants.Permission, permission));
            }

            return IdentityResult.Failed();
        }
    }
}