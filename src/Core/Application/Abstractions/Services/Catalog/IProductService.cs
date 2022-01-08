using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog;
using System;
using System.Threading.Tasks;
using StoreKit.Shared.DTOs.Catalog.Product;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface IProductService : ITransientService
    {
        Task<Result<ProductDetailsDto>> GetProductDetailsAsync(Guid id);
        Task<Result<ProductDto>> GetByIdUsingDapperAsync(Guid id);
        Task<PaginatedResult<ProductDto>> SearchAsync(ProductListFilter filter);
        Task<Result<Guid>> CreateProductAsync(CreateProductRequest request);
        Task<Result<Guid>> UpdateProductAsync(UpdateProductRequest request, Guid id);
        Task<Result<Guid>> DeleteProductAsync(Guid id);
    }
}