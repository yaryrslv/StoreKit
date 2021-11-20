using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    public class ProductsController : BaseController
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [MustHavePermission(PermissionConstants.Products.View)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var product = await _service.GetProductDetailsAsync(id);
            return Ok(product);
        }

        [HttpPost("search")]
        [MustHavePermission(PermissionConstants.Products.Search)]
        public async Task<IActionResult> SearchAsync(ProductListFilter filter)
        {
            var products = await _service.SearchAsync(filter);
            return Ok(products);
        }

        [HttpGet("dapper")]
        [MustHavePermission(PermissionConstants.Products.View)]
        public async Task<IActionResult> GetDapperAsync(Guid id)
        {
            var products = await _service.GetByIdUsingDapperAsync(id);
            return Ok(products);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Products.Register)]
        public async Task<IActionResult> CreateAsync(CreateProductRequest request)
        {
            return Ok(await _service.CreateProductAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.Products.Update)]
        public async Task<IActionResult> UpdateAsync(UpdateProductRequest request, Guid id)
        {
            return Ok(await _service.UpdateProductAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.Products.Remove)]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var productId = await _service.DeleteProductAsync(id);
            return Ok(productId);
        }
    }
}