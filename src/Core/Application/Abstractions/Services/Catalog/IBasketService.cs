using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog.Basket;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface IBasketService : ITransientService
    {
        Task<Result<BasketDetailsDto>> GetBasketDetailsAsync(Guid id);
        Task<BasketDetailsDto> GetBasketDetailsByUserIdAsync(Guid id);
        Task<Result<Guid>> CreateBasketAsync(CreateBasketRequest request, Guid userId);
        Task<Result<Guid>> UpdateBasketAsync(UpdateBasketRequest request, Guid userId);
        Task<Result<Guid>> DeleteBasketAsync(Guid id);
    }
}