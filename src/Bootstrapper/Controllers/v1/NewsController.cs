using System;
using StoreKit.Application.Abstractions.Services.Identity;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Identity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Catalog;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search News using available Filters.")]
        public async Task<IActionResult> SearchAsync(NewsListFilter filter, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var news = await _service.SearchAsync(filter);
            return Ok(news);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.News.Register)]
        public async Task<IActionResult> CreateAsync(CreateNewsRequest request)
        {
            return Ok(await _service.CreateNewsAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.News.Update)]
        public async Task<IActionResult> UpdateAsync(UpdateNewsRequest request, Guid id)
        {
            return Ok(await _service.UpdateNewsAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.News.Remove)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var newsId = await _service.DeleteNewsAsync(id);
            return Ok(newsId);
        }
    }
}