using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _service;

        public TagsController(ITagsService tagsService)
        {
            _service = tagsService;
        }

        [HttpGet("get-tag-type-by-product-id/{productId}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> GetTagTypeByProductAsync(Guid productId, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var tagType = await _service.GetTagTypeByProductAsync(productId);
            return Ok(tagType);
        }

        [HttpPost("post-tag-type-by-product")]
        [MustHavePermission(PermissionConstants.TagTypes.Create)]
        public async Task<IActionResult> PostTagTypeByProductAsync(PostTagTypeRequest request)
        {
            return Ok(await _service.PostTagTypeByProductAsync(request));
        }

        [HttpGet("get-tag-by-tag-type-name/{tagTypeName}")]
        [HttpGet("get-tag-type-by-product-id/{productId}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> GetTagsByTagTypeName(string tagTypeName, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var tagType = await _service.GetTagsByTagTypeName(tagTypeName);
            return Ok(tagType);
        }

        [HttpPost("post-tags-by-tag-type-name")]
        [MustHavePermission(PermissionConstants.TagTypes.Create)]
        public async Task<IActionResult> PostTagsByTagTypeName(PostTagsRequest request)
        {
            return Ok(await _service.PostTagsByTagTypeName(request));
        }

        [HttpGet("get-unique-tag-types")]
        [HttpGet("get-tag-type-by-product-id/{productId}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> GetUniqueTagTypes([FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            return Ok(await _service.GetUniqueTagTypes());
        }
    }
}