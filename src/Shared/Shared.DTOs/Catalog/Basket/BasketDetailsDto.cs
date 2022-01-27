using System;
using System.Collections;
using System.Collections.Generic;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog.Basket
{
    public class BasketDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<ProductInBasket> Products { get; set; }
    }
}