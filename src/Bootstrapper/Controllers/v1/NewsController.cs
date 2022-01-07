using System;
using System.Collections.Generic;
using StoreKit.Application.Abstractions.Services.Identity;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Identity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Catalog;
using StoreKit.Shared.DTOs.Catalog.News;

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

        [HttpPost("generate")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search News using available Filters.")]
        public async Task<IActionResult> GenerateAsync(int generationCount)
        {
            var testNewsGenerator = new Faker<CreateNewsRequest>()
                .RuleFor(u => u.Title, (f, u) => f.Lorem.Sentence())
                .RuleFor(u => u.Description, (f, u) => f.Lorem.Paragraph());
            var testNewsList = testNewsGenerator.Generate(generationCount);
            foreach (var testNewsItem in testNewsList)
            {
                await _service.CreateNewsAsync(testNewsItem);
            }

            return Ok();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<ProductDetailsDto>))]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var news = await _service.GetNewsDetailsAsync(id);
            return Ok(news);
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search News using available Filters.")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<NewsDto>))]
        public async Task<IActionResult> SearchAsync(NewsListFilter filter, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var news = await _service.SearchAsync(filter);
            return Ok(news);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.News.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreateNewsRequest request)
        {
            return Ok(await _service.CreateNewsAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.News.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdateNewsRequest request, Guid id)
        {
            return Ok(await _service.UpdateNewsAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.News.Delete)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var newsId = await _service.DeleteNewsAsync(id);
            return Ok(newsId);
        }
    }
}