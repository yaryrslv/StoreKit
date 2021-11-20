using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog;
using System;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface INewsService : ITransientService
    {
        Task<PaginatedResult<NewsDto>> SearchAsync(NewsListFilter filter);
        Task<Result<Guid>> CreateNewsAsync(CreateNewsRequest request);
        Task<Result<Guid>> UpdateNewsAsync(UpdateNewsRequest request, Guid id);
        Task<Result<Guid>> DeleteNewsAsync(Guid id);
    }
}