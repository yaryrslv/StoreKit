using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface IPageService : ITransientService
    {
        Task<Result<PageDetailsDto>> GetPageDetailsAsync(Guid id);
        Task<PaginatedResult<PageDto>> SearchAsync(PageListFilter filter);
        Task<Result<Guid>> CreatePageAsync(CreatePageRequest request);
        Task<Result<Guid>> UpdatePageAsync(UpdatePageRequest request, Guid id);
        Task<Result<Guid>> DeletePageAsync(Guid id);
    }
}