using Microsoft.AspNetCore.Authorization;

namespace StoreKit.Infrastructure.Identity.Permissions
{
    public class MustHavePermission : AuthorizeAttribute
    {
        public MustHavePermission(string permission)
        {
            Policy = permission;
        }
    }
}