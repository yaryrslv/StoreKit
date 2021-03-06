using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Multitenancy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.General
{
    public interface ITenantManager : ITransientService
    {
        public Task<Result<TenantDto>> GetByKeyAsync(string key);
        public Task<Result<List<TenantDto>>> GetAllAsync();
        public Task<Result<object>> CreateTenantAsync(CreateTenantRequest request);
        Task<Result<object>> UpgradeSubscriptionAsync(UpgradeSubscriptionRequest request);
        Task<Result<object>> DeactivateTenantAsync(string tenant);
        Task<Result<object>> ActivateTenantAsync(string tenant);
    }
}