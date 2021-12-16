using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Domain.Enums;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Application.Services.Catalog
{
    public class CommentsService : ICommentsService
    {
        private readonly IStringLocalizer<CommentsService> _localizer;
        private readonly IRepositoryAsync _repository;

        public CommentsService(IRepositoryAsync repository, IStringLocalizer<CommentsService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<PaginatedResult<CommentsDto>> SearchAsync(CommentsListFilter filter)
        {
            var newsCollection = await _repository.GetSearchResultsAsync<Comments, CommentsDto>(filter.PageNumber, filter.PageSize, filter.OrderBy, filter.Keyword);
            return newsCollection;
        }

        public async Task<Result<Guid>> CreateCommentsAsync(CreateCommentsRequest request)
        {
            var comments = new Comments(request.CommentatorName, request.Title, request.Description);
            var commentsId = await _repository.CreateAsync<Comments>(comments);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(commentsId);
        }

        public async Task<Result<Guid>> UpdateCommentsAsync(UpdateCommentsRequest request, Guid id)
        {
            var comment = await _repository.GetByIdAsync<Comments>(id, null);
            if (comment == null) throw new EntityNotFoundException(string.Format(_localizer["comment.notfound"], id));

            var updatedComment = comment.Update(request.CommentatorName, request.Description, request.Title);
            await _repository.UpdateAsync<Comments>(updatedComment);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeleteCommentsAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Comments>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}