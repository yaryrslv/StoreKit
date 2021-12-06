using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Exceptions;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Constants;
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

        public async Task<Result<TagType>> GetTagTypeByProductAsync(Guid productId)
        {
            var product = await _repository.GetByIdAsync<Product>(productId, null);
            if (product == null) throw new EntityNotFoundException(string.Format(_localizer["product.notfound"], productId));
            return await Result<TagType>.SuccessAsync(product.TagType);
        }

        public async Task<Result<Guid>> PostTagTypeByProductAsync(PostTagTypeRequest request)
        {
            var product = await _repository.GetByIdAsync<Product>(request.ProductId, null);
            if (product == null) throw new EntityNotFoundException(string.Format(_localizer["product.notfound"], request.ProductId));
            product.TagType = request.TagType;
            await _repository.UpdateAsync(product);
            return await Result<Guid>.SuccessAsync(product.Id);
        }
        public async Task<Result<List<Tag>>> GetTagsByTagTypeName(string tagTypeName)
        {
            var productList = await _repository.GetListAsync<Product>(null);
            var tagTypeTagList = productList.FirstOrDefault(i => i.TagType.Name == tagTypeName)?.TagType.Tags;

            if (tagTypeTagList == null) throw new EntityNotFoundException(string.Format(_localizer["tagname.notfound"], tagTypeName));
            return await Result<List<Tag>>.SuccessAsync(tagTypeTagList);
        }
        public async Task<Result<List<Tag>>> PostTagsByTagTypeName(PostTagsRequest request)
        {
            var productList = await _repository.GetListAsync<Product>(null);
            var tagTypeProducts = productList.Where(i => i.TagType.Name == request.TagName);
            foreach (var tagTypeProduct in tagTypeProducts)
            {
                tagTypeProduct.TagType.Tags.AddRange(request.Tags);
                await _repository.UpdateAsync(tagTypeProduct);
            }
            if (productList == null) throw new EntityNotFoundException(string.Format(_localizer["productList.notfound"], request.TagName));
            if (tagTypeProducts == null) throw new EntityNotFoundException(string.Format(_localizer["tagTypeProducts.notfound"], request.TagName));
            return await Result<List<Tag>>.SuccessAsync(tagTypeProducts.FirstOrDefault().TagType.Tags);
        }
        public async Task<Result<List<TagType>>> GetUniqueTagTypes()
        {
            var productList = await _repository.GetListAsync<Product>(null);
            var uniqueTagTypes = productList.Select(i => i.TagType).Distinct();
            if (productList == null) throw new EntityNotFoundException(string.Format(_localizer["productList.notfound"], productList));
            if (uniqueTagTypes == null) throw new EntityNotFoundException(string.Format(_localizer["uniqutTagTypes.notfound"],  uniqueTagTypes));
            return await Result<List<TagType>>.SuccessAsync(uniqueTagTypes);
        }
    }
}