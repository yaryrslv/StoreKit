using System;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class CategoryDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}