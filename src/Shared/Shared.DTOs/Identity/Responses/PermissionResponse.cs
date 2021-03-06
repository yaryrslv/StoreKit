using System.Collections.Generic;

namespace StoreKit.Shared.DTOs.Identity.Responses
{
    public class PermissionResponse
    {
        public string RoleId { get; set; }

        public string RoleName { get; set; }

        public List<RoleClaimResponse> RoleClaims { get; set; }
    }
}