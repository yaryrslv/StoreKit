using System;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class NewsDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationTime { get; set; }
    }
}