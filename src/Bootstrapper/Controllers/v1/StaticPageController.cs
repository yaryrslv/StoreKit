using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Wrapper;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Catalog.StaticPage;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class StaticPageController : ControllerBase
{
    private readonly IStaticPageService _staticPageService;

    public StaticPageController(IStaticPageService staticPageService)
    {
        _staticPageService = staticPageService;
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
    [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<StaticPageDto>))]
    public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
    {
        var page = await _staticPageService.GetStaticPageAsync(id);
        return Ok(page.Body);
    }
}