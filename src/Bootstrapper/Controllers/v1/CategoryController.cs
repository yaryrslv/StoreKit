using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Catalog;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search News using available Filters.")]
        public async Task<IActionResult> GenerateAsync(int generationCount)
        {
            var testCategoryGenerator = new Faker<CreateCategoryRequest>("ru_RU")
                .RuleFor(u => u.Name, (f, u) => f.Commerce.Product() + Guid.NewGuid());
            var testCategoryList = testCategoryGenerator.Generate(generationCount);
            foreach (var testNewsItem in testCategoryList)
            {
                await _service.CreateCategoryAsync(testNewsItem);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<CategoryDetailsDto>))]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var category = await _service.GetCategoryDetailsAsync(id);
            return Ok(category);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search Category using available Filters.")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<CategoryDto>))]
        public async Task<IActionResult> SearchAsync(CategoryListFilter filter, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var category = await _service.SearchAsync(filter);
            return Ok(category);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Categories.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreateCategoryRequest request)
        {
            return Ok(await _service.CreateCategoryAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.Categories.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdateCategoryRequest request, Guid id)
        {
            return Ok(await _service.UpdateCategoryAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.Categories.Delete)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var categoryId = await _service.DeleteCategoryAsync(id);
            return Ok(categoryId);
        }
    }
}