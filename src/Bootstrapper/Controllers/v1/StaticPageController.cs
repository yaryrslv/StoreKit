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
using StoreKit.Shared.DTOs.Catalog.StaticPage;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class StaticPageController : ControllerBase
    {
        private readonly IStaticPageService _service;

        public StaticPageController(IStaticPageService staticPageService)
        {
            _service = staticPageService;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string))]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var page = await _service.GetStaticPageDetailsAsync(id);
            return Ok(page.Data.Body);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.StaticPages.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreateStaticPageRequest request)
        {
            return Ok(await _service.CreateStaticPageAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.StaticPages.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdateStaticPageRequest request, Guid id)
        {
            return Ok(await _service.UpdateStaticPageAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.StaticPages.Delete)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var newsId = await _service.DeleteStaticPageAsync(id);
            return Ok(newsId);
        }
    }
}