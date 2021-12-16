using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace StoreKit.Application.Services.Catalog
{
    public class NewsService : INewsService
    {
        private readonly IStringLocalizer<NewsService> _localizer;
        private readonly IRepositoryAsync _repository;

        public NewsService(IRepositoryAsync repository, IStringLocalizer<NewsService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<Result<Guid>> CreateNewsAsync(CreateNewsRequest request)
        {
            var newsExists = await _repository.ExistsAsync<News>(a => a.Title == request.Title);
            //if (brandExists) throw new EntityAlreadyExistsException(string.Format(_localizer["news.alreadyexists"], request.Title));
            var news = new News(request.Title, request.Description);
            var newsId = await _repository.CreateAsync<News>(news);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(newsId);
        }

        public async Task<Result<Guid>> DeleteNewsAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<News>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<PaginatedResult<NewsDto>> SearchAsync(NewsListFilter filter)
        {
            var newsCollection = await _repository.GetSearchResultsAsync<News, NewsDto>(filter.PageNumber, filter.PageSize, filter.OrderBy, filter.Keyword);
            return newsCollection;
        }

        public async Task<Result<Guid>> UpdateNewsAsync(UpdateNewsRequest request, Guid id)
        {
            var brand = await _repository.GetByIdAsync<News>(id);
            if (brand == null) throw new EntityNotFoundException(string.Format(_localizer["news.notfound"], id));
            var updatedBrand = brand.Update(request.Title, request.Description);
            await _repository.UpdateAsync<News>(updatedBrand);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}