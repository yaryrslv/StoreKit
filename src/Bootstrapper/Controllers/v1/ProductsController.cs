using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using StoreKit.Application.Abstractions.Repositories;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Catalog.Product;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IRepositoryAsync _repositoryAsync;

        public ProductsController(IProductService service, IRepositoryAsync repositoryAsync)
        {
            _service = service;
            _repositoryAsync = repositoryAsync;
        }

        [HttpPost("generate")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> GenerateAsync(int generationCount)
        {
            var categories = await _repositoryAsync.GetSearchResultsAsync<Category, CategoryDetailsDto>(0, 888);
            if (categories.TotalCount <= 0)
            {
                return NotFound("Categories not found");
            }

            var testProductsGnerator = new Faker<CreateProductRequest>()
                .RuleFor(u => u.Name, (f, u) => f.Commerce.ProductName())
                .RuleFor(u => u.Description, (f, u) => f.Commerce.ProductDescription())
                .RuleFor(u => u.CategoryId, (f, u) => categories.Data[new Random().Next(categories.Data.Count - 1)].Id);
            var testProductsList = testProductsGnerator.Generate(generationCount);
            foreach (var testProductItem in testProductsList)
            {
                var testTagsGenerator = new Faker<Tag>()
                    .RuleFor(u => u.Name, (f, u) => f.Commerce.ProductName())
                    .RuleFor(u => u.Value, (f, u) => f.Commerce.Price());
                testProductItem.Tags = testTagsGenerator.Generate(8);
                await _service.CreateProductAsync(testProductItem, null);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<ProductDetailsDto>))]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var product = await _service.GetProductDetailsAsync(id);
            return Ok(product);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<ProductDto>))]
        public async Task<IActionResult> SearchAsync(ProductListFilter filter, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var products = await _service.SearchAsync(filter);
            return Ok(products);
        }

        [HttpGet("dapper")]
        [MustHavePermission(PermissionConstants.TagTypes.View)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<ProductDto>))]
        public async Task<IActionResult> GetDapperAsync(Guid id)
        {
            var products = await _service.GetByIdUsingDapperAsync(id);
            return Ok(products);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Products.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreateProductRequest request)
        {
            var files = HttpContext.Request.Form.Files;
            if (files.Count <= 0)
            {
                return BadRequest("File not found");
            }

            var imageStream = files[0].OpenReadStream();
            return Ok(await _service.CreateProductAsync(request, imageStream));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.Products.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdateProductRequest request, Guid id)
        {
            return Ok(await _service.UpdateProductAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.Products.Delete)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var productId = await _service.DeleteProductAsync(id);
            return Ok(productId);
        }
    }
}