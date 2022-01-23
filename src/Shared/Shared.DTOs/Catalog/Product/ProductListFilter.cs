using System;
using System.Collections.Generic;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Filters;

namespace StoreKit.Shared.DTOs.Catalog.Product
{
    public class ProductListFilter : PaginationFilter
    {
        public Guid? CategoryId { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ProductPrice> Prices { get; set; }
    }
}