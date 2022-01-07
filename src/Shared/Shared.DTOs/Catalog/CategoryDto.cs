using System;
using System.Collections.Generic;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class CategoryDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}