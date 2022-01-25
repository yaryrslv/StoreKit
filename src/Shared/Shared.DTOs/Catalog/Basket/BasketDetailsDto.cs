using System;
using System.Collections.Generic;

namespace StoreKit.Shared.DTOs.Catalog.Basket
{
    public class BasketDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public List<Domain.Entities.Catalog.Product> Products { get; set; }
    }
}