using System;

namespace StoreKit.Shared.DTOs.Catalog.StaticPage
{
    public class StaticPageDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
    }
}