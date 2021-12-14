using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface ICommentsService : ITransientService
    {
        Task<PaginatedResult<CommentsDto>> SearchAsync(CommentsListFilter filter);
        Task<Result<Guid>> CreateCommentsAsync(CreateCommentsRequest request);
        Task<Result<Guid>> UpdateCommentsAsync(UpdateCommentsRequest request, Guid id);
        Task<Result<Guid>> DeleteCommentsAsync(Guid id);
    }
}