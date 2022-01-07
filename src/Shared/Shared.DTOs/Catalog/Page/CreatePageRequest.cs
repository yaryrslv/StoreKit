using StoreKit.Domain.Enums;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class CreatePageRequest : IMustBeValid
    {
        public string Name { get; set; }
        public PageType PageType { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }
    }
}