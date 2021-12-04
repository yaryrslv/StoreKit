using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Application.Services.Catalog
{
    public class TagsService : ITagsService
    {
        private readonly IRepositoryAsync _repository;
        private readonly IStringLocalizer<TagsService> _localizer;

        public TagsService(IRepositoryAsync repository, IStringLocalizer<TagsService> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        public async Task<Result<TagType>> GetTagTypeAsync(GetTagsRequest request)
        {
            var product = await _repository.GetByIdAsync<Product>(request.ProductId, null);
            if (product == null) throw new EntityNotFoundException(string.Format(_localizer["product.notfound"], request.ProductId));
            return await Result<TagType>.SuccessAsync(product.TagType);
        }

        public async Task<Result<Guid>> PostTagTypeAsync(PostTagsRequest request, TagType tagType)
        {
            var product = await _repository.GetByIdAsync<Product>(request.ProductId, null);
            if (product == null) throw new EntityNotFoundException(string.Format(_localizer["product.notfound"], request.ProductId));
            product.TagType = request.TagType;
            await _repository.UpdateAsync(product);
            return await Result<Guid>.SuccessAsync(product.Id);
        }
    }
}