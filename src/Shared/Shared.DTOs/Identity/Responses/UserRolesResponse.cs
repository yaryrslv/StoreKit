using System.Collections.Generic;

namespace StoreKit.Shared.DTOs.Identity.Responses
{
    public class UserRolesResponse
    {
        public List<UserRoleDto> UserRoles { get; set; } = new();
    }
}