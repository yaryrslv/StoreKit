using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace StoreKit.Application.Abstractions.Services.Identity
{
    public interface ICurrentUser : ITransientService
    {
        string Name { get; }
        Guid GetUserId();
        string GetUserEmail();
        string GetTenantKey();
        bool IsAuthenticated();
        bool IsInRole(string role);
        IEnumerable<Claim> GetUserClaims();
    }
}