using StoreKit.Domain.Contracts;
using StoreKit.Domain.Enums;

namespace StoreKit.Domain.Entities.Catalog
{
    public class Page : AuditableEntity
    {
        public string Name { get; set; }
        public PageType PageType { get; set; }
        public string Url { get; set; }

        public Page(string name, PageType pageType, string url)
        {
            Name = name;
            PageType = pageType;
            Url = url;
        }
        public Page Update(string name, PageType pageType, string url)
        {
            if (!string.IsNullOrEmpty(name)) Name = name;
            PageType = pageType;
            if (!string.IsNullOrEmpty(url)) Url = url;
            return this;
        }
    }
}