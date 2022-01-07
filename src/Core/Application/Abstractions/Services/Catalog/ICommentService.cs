using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface ICommentService : ITransientService
    {
        Task<Result<CommentDetailsDto>> GetCommentDetailsAsync(Guid id);
        Task<PaginatedResult<CommentDto>> SearchAsync(CommentsListFilter filter);
        Task<Result<Guid>> CreateCommentsAsync(CreateCommentsRequest request);
        Task<Result<Guid>> UpdateCommentsAsync(UpdateCommentsRequest request, Guid id);
        Task<Result<Guid>> DeleteCommentsAsync(Guid id);
    }
}