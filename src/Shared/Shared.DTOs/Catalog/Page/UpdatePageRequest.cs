using StoreKit.Domain.Enums;

namespace StoreKit.Shared.DTOs.Catalog.Page
{
    public class UpdatePageRequest : IMustBeValid
    {
        public string Name { get; set; }
        public PageType PageType { get; set; }
        public string Url { get; set; }
    }
}