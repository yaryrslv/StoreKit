using System;

namespace StoreKit.Shared.DTOs.Catalog.StaticPage
{
    public class StaticPageDto
    {
        public Guid Id { get; set; }
        public string Body { get; set; }
    }

    public class CreateStaticPageDto
    {
        public string Body { get; set; }
    }
}