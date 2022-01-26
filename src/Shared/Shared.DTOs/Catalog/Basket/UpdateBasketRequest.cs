using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace StoreKit.Shared.DTOs.Catalog.Basket
{
    public class UpdateBasketRequest : IMustBeValid
    {
        public List<ProductsInBasketDto> Products { get; set; }
    }
}