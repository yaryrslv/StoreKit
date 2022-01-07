using System;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Catalog;
using StoreKit.Shared.DTOs.Catalog.Comment;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface ICommentService : ITransientService
    {
        Task<Result<CommentDetailsDto>> GetCommentDetailsAsync(Guid id);
        Task<PaginatedResult<CommentDto>> SearchAsync(CommentListFilter filter);
        Task<Result<Guid>> CreateCommentsAsync(CreateCommentRequest request);
        Task<Result<Guid>> UpdateCommentsAsync(UpdateCommentRequest request, Guid id);
        Task<Result<Guid>> DeleteCommentsAsync(Guid id);
    }
}