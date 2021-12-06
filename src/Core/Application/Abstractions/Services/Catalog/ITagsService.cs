using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Application.Abstractions.Services.Catalog
{
    public interface ITagsService
    {
        Task<Result<TagType>> GetTagTypeByProductAsync(Guid productId);
        Task<Result<Guid>> PostTagTypeByProductAsync(PostTagTypeRequest request);
        Task<Result<List<Tag>>> GetTagsByTagTypeName(string TagTypeName);
        Task<Result<List<Tag>>> PostTagsByTagTypeName(PostTagsRequest request);
        Task<Result<List<TagType>>> GetUniqueTagTypes();

    }
}