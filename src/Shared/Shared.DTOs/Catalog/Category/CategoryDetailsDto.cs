using System;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class CategoryDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}