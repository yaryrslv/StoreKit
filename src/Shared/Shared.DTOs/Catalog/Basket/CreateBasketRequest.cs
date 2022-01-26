using System;
using System.Collections.Generic;

namespace StoreKit.Shared.DTOs.Catalog.Basket
{
    public class CreateBasketRequest : IMustBeValid
    {
        public List<ProductsInBasketDto> Products { get; set; }
    }
}