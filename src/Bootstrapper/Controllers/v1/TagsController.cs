using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    public class TagsController : BaseController
    {
        private readonly ITagsService _service;

        public TagsController(ITagsService tagsService)
        {
            _service = tagsService;
        }

        [HttpGet("get-tag-type-by-product-id/{productId}")]
        public async Task<IActionResult> GetTagTypeByProductAsync(Guid productId)
        {
            var tagType = await _service.GetTagTypeByProductAsync(productId);
            return Ok(tagType);
        }

        [HttpPost("post-tag-type-by-product")]
        public async Task<IActionResult> PostTagTypeByProductAsync(PostTagTypeRequest request)
        {
            return Ok(await _service.PostTagTypeByProductAsync(request));
        }

        [HttpGet("get-tag-by-tag-type-name/{tagTypeName}")]
        public async Task<IActionResult> GetTagsByTagTypeName(string tagTypeName)
        {
            var tagType = await _service.GetTagsByTagTypeName(tagTypeName);
            return Ok(tagType);
        }

        [HttpPost("post-tags-by-tag-type-name")]
        public async Task<IActionResult> PostTagsByTagTypeName(PostTagsRequest request)
        {
            return Ok(await _service.PostTagsByTagTypeName(request));
        }

        [HttpGet("get-unique-tag-types")]
        public async Task<IActionResult> GetUniqueTagTypes()
        {
            return Ok(await _service.GetUniqueTagTypes());
        }
    }
}