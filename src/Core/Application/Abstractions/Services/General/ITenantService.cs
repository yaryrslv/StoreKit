using StoreKit.Shared.DTOs.Multitenancy;

namespace StoreKit.Application.Abstractions.Services.General
{
    public interface ITenantService : IScopedService
    {
        public string GetDatabaseProvider();
        public string GetConnectionString();
        public TenantDto GetCurrentTenant();
    }
}