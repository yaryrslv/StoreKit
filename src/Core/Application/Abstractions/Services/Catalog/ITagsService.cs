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
        Task<Result<TagType>> GetTagTypeAsync(GetTagsRequest request);
        Task<Result<Guid>> PostTagTypeAsync(PostTagsRequest request, TagType tagType);
    }
}