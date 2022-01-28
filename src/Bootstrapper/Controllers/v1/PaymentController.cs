using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OPSPay.Client;
using OPSPay.Client.Exceptions;
using OPSPay.Client.Types;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using StoreKit.Application.Abstractions.Services.Catalog;
using StoreKit.Application.Wrapper;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Infrastructure.SwaggerFilters;
using StoreKit.Shared.DTOs.Catalog.Basket;
using Swashbuckle.AspNetCore.Annotations;

namespace StoreKit.Bootstrapper.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IOpspay _opspay;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBasketService _service;

        public PaymentController(ILogger<PaymentController> logger, IOpspay opspay, IHttpContextAccessor httpContextAccessor, IBasketService service)
        {
            _logger = logger;
            _opspay = opspay;
            _httpContextAccessor = httpContextAccessor;
            _service = service;
        }

        /// <summary>
        /// Создать платеж.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Type = typeof(string))]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> PayAsync(int totalCount = 0, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var basket = await _service.GetBasketDetailsByUserIdAsync(new Guid(userId));

            int orderId = new Random().Next(1000, 10000);

            // счет в платеже
            Order order = new Order
            {
                // MerchantId = 6,
                OrderId = orderId,
                OrderNumber = $"StoreKit-{orderId}",
                OrderDate = DateTime.Now,
                Items = new List<OrderItem>
                        {
                            new OrderItem
                            {
                                ProductId = totalCount == 0 ? Guid.NewGuid().ToString() : basket.Products[0].Id.ToString(),
                                Description = "Покупатель: " + userId,
                                Price = totalCount == 0 ? totalCount : (int)(basket.Products.Sum(i => i.Price) * 100)

                                // Quantity = 1,
                                // Unit = "шт."
                            }
                        },
            };

            // Общие параметры платежа
            Payment payment = new Payment()
            {
                IsTest = true,
                Orders = new List<Order> { order }
            };

            string mess = "All well done!!!";
            try
            {
                // создать платеж в OPS Pay
                var result = await _opspay.CreatePaymentAsync(payment);

                // сделать редирект на оплату в процессинг OPS Pay
                if (result.State == PaymentState.Added)
                    return new OkObjectResult(_opspay.GetUrlRedirectToPay(result.PaymentNumber!));
            }
            catch (OpsException ex)
            {
                mess = ex.Message;
                return new ObjectResult(mess);
            }

            await _service.DeleteBasketAsync(basket.Id);
            return new ObjectResult(mess);
        }
    }
}