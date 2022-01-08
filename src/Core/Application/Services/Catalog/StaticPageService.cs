using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Specifications;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog.StaticPage;

namespace StoreKit.Application.Services.Catalog
{
    public class StaticPageService : IStaticPageService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IStringLocalizer<StaticPageService> _localizer;

        public StaticPageService(IRepositoryAsync repository, IStringLocalizer<StaticPageService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }
        public async Task<Guid> CreateStaticPageAsync(CreateStaticPageRequest request)
        {
            var staticPage = new StaticPage(request.Body);
            var guid = await _repository.CreateAsync(staticPage);
            await _repository.SaveChangesAsync();
            return guid;
        }

        public async Task<Result<StaticPageDetailsDto>> GetStaticPageDetailsAsync(Guid id)
        {
            var spec = new BaseSpecification<StaticPage>();
            var page = await _repository.GetByIdAsync<StaticPage, StaticPageDetailsDto>(id, spec);
            return await Result<StaticPageDetailsDto>.SuccessAsync(page);
        }

        public async Task<Result<Guid>> UpdateStaticPageAsync(UpdateStaticPageRequest request, Guid id)
        {
            var staticPage = await _repository.GetByIdAsync<StaticPage>(id);
            if (staticPage == null) throw new EntityNotFoundException(string.Format(_localizer["staticpage.notfound"], id));
            var updatedPage = staticPage.Update(request.Body);
            await _repository.UpdateAsync<StaticPage>(updatedPage);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeleteStaticPageAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<StaticPage>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}