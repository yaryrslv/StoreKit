using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Abstractions.Services.Identity;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Catalog.Basket;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserService _userService;

        public BasketController(IBasketService service, IHttpContextAccessor httpContextAccessor, IUserService userService)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        [HttpGet]
        [MustHavePermission(PermissionConstants.Baskets.View)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<BasketDto>))]
        public async Task<IActionResult> GetAsync([FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var basket = await _service.GetBasketDetailsByUserIdAsync(new Guid(userId));
            return Ok(basket);
        }

        [HttpPut]
        [MustHavePermission(PermissionConstants.Baskets.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdateBasketRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _service.UpdateBasketAsync(request, new Guid(userId)));
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Baskets.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreateBasketRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return Ok(await _service.CreateBasketAsync(request, new Guid(userId)));
        }

        [HttpDelete("{id}")]
        [MustHavePermission(PermissionConstants.Baskets.Delete)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var basketId = await _service.DeleteBasketAsync(id);
            return Ok(basketId);
        }
    }
}