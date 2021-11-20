using System.Collections.Generic;

namespace StoreKit.Shared.DTOs.Identity.Requests
{
    public class UserRolesRequest
    {
        public List<UserRoleDto> UserRoles { get; set; } = new();
    }
}