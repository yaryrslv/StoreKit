using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading.Tasks;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    public class NewsController : BaseController
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [HttpPost("search")]
        [MustHavePermission(PermissionConstants.Brands.Search)]
        [SwaggerOperation(Summary = "Search News using available Filters.")]
        public async Task<IActionResult> SearchAsync(NewsListFilter filter)
        {
            var brands = await _service.SearchAsync(filter);
            return Ok(brands);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Brands.Register)]
        public async Task<IActionResult> CreateAsync(CreateNewsRequest request)
        {
            return Ok(await _service.CreateNewsAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.Brands.Update)]
        public async Task<IActionResult> UpdateAsync(UpdateNewsRequest request, Guid id)
        {
            return Ok(await _service.UpdateNewsAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.Brands.Remove)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var brandId = await _service.DeleteNewsAsync(id);
            return Ok(brandId);
        }
    }
}