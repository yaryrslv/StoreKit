using System;
using System.Collections.Generic;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.Filters;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class ProductListFilter : PaginationFilter
    {
        public Guid? CategoryId { get; set; }
        public List<Tag> Tags { get; set; }
    }
}