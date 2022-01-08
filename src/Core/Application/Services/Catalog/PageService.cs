using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Specifications;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog;
using StoreKit.Shared.DTOs.Catalog.Page;

namespace StoreKit.Application.Services.Catalog
{
    public class PageService : IPageService
    {
        private readonly IStringLocalizer<PageService> _localizer;
        private readonly IRepositoryAsync _repository;

        public PageService(IRepositoryAsync repository, IStringLocalizer<PageService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<Result<PageDetailsDto>> GetPageDetailsAsync(Guid id)
        {
            var spec = new BaseSpecification<Page>();
            var page = await _repository.GetByIdAsync<Page, PageDetailsDto>(id, spec);
            return await Result<PageDetailsDto>.SuccessAsync(page);
        }

        public async Task<PaginatedResult<PageDto>> SearchAsync(PageListFilter filter)
        {
            var pageCollection = await _repository.GetSearchResultsAsync<Page, PageDto>(filter.PageNumber,
                filter.PageSize, filter.OrderBy, filter.Keyword);
            return pageCollection;
        }

        public async Task<Result<Guid>> CreatePageAsync(CreatePageRequest request)
        {
            var pageExists =
                await _repository.ExistsAsync<Page>(a => a.Name == request.Name);
            if (pageExists)
                throw new EntityAlreadyExistsException(string.Format(_localizer["page.alreadyexists"], request.Name));
            var page = new Page(request.Name, request.PageType, request.Url);
            var pageId = await _repository.CreateAsync<Page>(page);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(pageId);
        }

        public async Task<Result<Guid>> UpdatePageAsync(UpdatePageRequest request, Guid id)
        {
            var page = await _repository.GetByIdAsync<Page>(id);
            if (page == null) throw new EntityNotFoundException(string.Format(_localizer["page.notfound"], id));
            var updatedPage = page.Update(request.Name, request.PageType, request.Url);
            await _repository.UpdateAsync<Page>(updatedPage);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeletePageAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Page>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}