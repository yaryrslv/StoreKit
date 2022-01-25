using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Infrastructure.Services;
using StoreKit.Shared.DTOs.Catalog.Basket;
using StoreKit.Shared.DTOs.Catalog.News;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _service;
        private readonly TestDataProvider _dataProvider;

        public BasketController(IBasketService service, TestDataProvider dataProvider)
        {
            _service = service;
            _dataProvider = dataProvider;
        }

        [HttpGet("{id}")]
        [MustHavePermission(PermissionConstants.Baskets.View)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<BasketDto>))]
        public async Task<IActionResult> GetAsync(Guid id, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var basket = await _service.GetBasketDetailsAsync(id);
            return Ok(basket);
        }

        [HttpGet("byuserid/{userId}")]
        [MustHavePermission(PermissionConstants.Baskets.View)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(PaginatedResult<BasketDto>))]
        public async Task<IActionResult> GetByUserIdAsync(Guid userId, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            var basket = await _service.GetBasketDetailsByUserIdAsync(userId);
            return Ok(basket);
        }

        [HttpPut("{id}")]
        [MustHavePermission(PermissionConstants.Baskets.Edit)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> UpdateAsync(UpdateBasketRequest request, Guid id)
        {
            return Ok(await _service.UpdateBasketAsync(request, id));
        }

        [HttpPost]
        [MustHavePermission(PermissionConstants.Baskets.Create)]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(Result<Guid>))]
        public async Task<IActionResult> CreateAsync(CreateBasketRequest request)
        {
            return Ok(await _service.CreateBasketAsync(request));
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