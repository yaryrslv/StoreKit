using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface ICategoryService : ITransientService
    {
        Task<Result<CategoryDetailsDto>> GetCategoryDetailsAsync(Guid id);
        Task<PaginatedResult<CategoryDto>> SearchAsync(CategoryListFilter filter);
        Task<Result<Guid>> CreateCategoryAsync(CreateCategoryRequest request);
        Task<Result<Guid>> UpdateCategoryAsync(UpdateCategoryRequest request, Guid id);
        Task<Result<Guid>> DeleteCategoryAsync(Guid id);
    }
}