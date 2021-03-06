using System;
using StoreKit.Domain.Enums;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class PageDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public PageType PageType { get; set; }
        public string Url { get; set; }
    }
}