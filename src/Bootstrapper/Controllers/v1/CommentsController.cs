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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _service;

        public CommentsController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<CommentDetailsDto>))]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var comment = await _service.GetCommentDetailsAsync(id);
            return Ok(comment);
        }

        [HttpPost("generate")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search Comments using available Filters.")]
        public async Task<IActionResult> GenerateAsync(int generationCount)
        {
            var testCommentsGenerator = new Faker<CreateCommentsRequest>()
                .RuleFor(u => u.CommentatorName, (f, u) => f.Person.UserName)
                .RuleFor(u => u.Title, (f, u) => f.Lorem.Sentence())
                .RuleFor(u => u.Description, (f, u) => f.Lorem.Paragraph());
            var testCommentsList = testCommentsGenerator.Generate(generationCount);
            foreach (var testCommentsItem in testCommentsList)
            {
                await _service.CreateCommentsAsync(testCommentsItem);
            }

            return Ok();
        }

        [HttpPost("search")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        [SwaggerOperation(Summary = "Search Comments using available Filters.")]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<CommentDto>))]
        public async Task<IActionResult> SearchAsync(CommentsListFilter filter, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var comments = await _service.SearchAsync(filter);
            return Ok(comments);
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Comments.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreateCommentsRequest request)
        {
            return Ok(await _service.CreateCommentsAsync(request));
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.Comments.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdateCommentsRequest request, Guid id)
        {
            return Ok(await _service.UpdateCommentsAsync(request, id));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.Comments.Delete)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var commentsId = await _service.DeleteCommentsAsync(id);
            return Ok(commentsId);
        }
    }
}