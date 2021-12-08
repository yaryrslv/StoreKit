using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StoreKit.Infrastructure.SwaggerFilters;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var product = await _service.GetProductDetailsAsync(id);
            return Ok(product);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> SearchAsync(ProductListFilter filter, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var products = await _service.SearchAsync(filter);
            return Ok(products);
        }

        [HttpGet("dapper")]
        [MustHavePermission(PermissionConstants.TagTypes.View)]
        public async Task<IActionResult> GetDapperAsync(Guid id)
        {
            var products = await _service.GetByIdUsingDapperAsync(id);
            return Ok(products);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Products.Create)]
        public async Task<IActionResult> CreateAsync(CreateProductRequest request)
        {
            return Ok(await _service.CreateProductAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.Products.Edit)]
        public async Task<IActionResult> UpdateAsync(UpdateProductRequest request, Guid id)
        {
            return Ok(await _service.UpdateProductAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.Products.Delete)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var productId = await _service.DeleteProductAsync(id);
            return Ok(productId);
        }
    }
}