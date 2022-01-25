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

namespace StoreKit.Application.Services.Catalog
{
    public class CategoryService : ICategoryService
    {
        private readonly IStringLocalizer<CategoryService> _localizer;
        private readonly IRepositoryAsync _repository;

        public CategoryService(IRepositoryAsync repository, IStringLocalizer<CategoryService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<Result<CategoryDetailsDto>> GetCategoryDetailsAsync(Guid id)
        {
            var spec = new BaseSpecification<Category>();
            var category = await _repository.GetByIdAsync<Category, CategoryDetailsDto>(id, spec);
            return await Result<CategoryDetailsDto>.SuccessAsync(category);
        }

        public async Task<PaginatedResult<CategoryDto>> SearchAsync(CategoryListFilter filter)
        {
            var categories = await _repository.GetSearchResultsAsync<Category, CategoryDto>(filter.PageNumber, filter.PageSize, filter.OrderBy, filter.Keyword);
            return categories;
        }


        public async Task<Result<Guid>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var categoryExists = await _repository.ExistsAsync<Category>(a => a.Name == request.Name);
            if (categoryExists) throw new EntityAlreadyExistsException(string.Format(_localizer["category.alreadyexists"], request.Name));
            var category = new Category(request.Name);
            var categoryId = await _repository.CreateAsync<Category>(category);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(categoryId);
        }

        public async Task<Result<Guid>> UpdateCategoryAsync(UpdateCategoryRequest request, Guid id)
        {
            var category = await _repository.GetByIdAsync<Category>(id);
            if (category == null) throw new EntityNotFoundException(string.Format(_localizer["category.notfound"], id));
            var updatedCategory = category.Update(request.Name);
            await _repository.UpdateAsync<Category>(updatedCategory);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }

        public async Task<Result<Guid>> DeleteCategoryAsync(Guid id)
        {
            await _repository.RemoveByIdAsync<Category>(id);
            await _repository.SaveChangesAsync();
            return await Result<Guid>.SuccessAsync(id);
        }
    }
}