using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Catalog;
using StoreKit.Shared.DTOs.Catalog.Page;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PageController : ControllerBase
    {
        private readonly IPageService _service;

        public PageController(IPageService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<PageDetailsDto>))]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var pages = await _service.GetPageDetailsAsync(id);
            return Ok(pages);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search News using available Filters.")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<PageDto>))]
        public async Task<IActionResult> SearchAsync(PageListFilter filter, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var pages = await _service.SearchAsync(filter);
            return Ok(pages);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.News.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreatePageRequest request)
        {
            return Ok(await _service.CreatePageAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.News.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdatePageRequest request, Guid id)
        {
            return Ok(await _service.UpdatePageAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.News.Delete)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var newsId = await _service.DeletePageAsync(id);
            return Ok(newsId);
        }
    }
}