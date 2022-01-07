using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Specifications;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Domain.Enums;
using StoreKit.Shared.DTOs.Catalog;
using StoreKit.Shared.DTOs.Catalog.Comment;

namespace StoreKit.Application.Services.Catalog
{
    public class CommentService : ICommentService
    {
        private readonly IStringLocalizer<CommentService> _localizer;
        private readonly IRepositoryAsync _repository;

        public CommentService(IRepositoryAsync repository, IStringLocalizer<CommentService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<Result<CommentDetailsDto>> GetCommentDetailsAsync(Guid id)
        {
            var spec = new BaseSpecification<Comment>();
            var comment = await _repository.GetByIdAsync<Comment, CommentDetailsDto>(id, spec);
            return await Result<CommentDetailsDto>.SuccessAsync(comment);
        }
        public async Task<PaginatedResult<CommentDto>> SearchAsync(CommentListFilter filter)
        {
            var newsCollection = await _repository.GetSearchResultsAsync<Comment, CommentDto>(filter.PageNumber, filter.PageSize, filter.OrderBy, filter.Keyword);
            return newsCollection;
        }

        public async Task<Result<Guid>> CreateCommentsAsync(CreateCommentRequest request)
        {
            var comments = new Comment(request.CommentatorName, request.Title, request.Description);
            var commentsId = await _repository.CreateAsync<Comment>(comments);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(commentsId);
        }

        public async Task<Result<Guid>> UpdateCommentsAsync(UpdateCommentRequest request, Guid id)
        {
            var comment = await _repository.GetByIdAsync<Comment>(id, null);
            if (comment == null) throw new EntityNotFoundException(string.Format(_localizer["comment.notfound"], id));

            var updatedComment = comment.Update(request.CommentatorName, request.Description, request.Title);
            await _repository.UpdateAsync<Comment>(updatedComment);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeleteCommentsAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Comment>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}