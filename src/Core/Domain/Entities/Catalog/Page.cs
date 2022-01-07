using StoreKit.Domain.Contracts;
using StoreKit.Domain.Enums;

namespace StoreKit.Domain.Entities.Catalog
{
    public class Page : AuditableEntity
    {
        public string Name { get; set; }
        public PageType PageType { get; set; }
        public string Content { get; set; }
        public string Url { get; set; }

        public Page(string name, PageType pageType, string content, string url)
        {
            Name = name;
            PageType = pageType;
            Content = content;
            Url = url;
        }
        public Page Update(string name, PageType pageType, string content, string url)
        {
            if (!string.IsNullOrEmpty(name)) Name = name;
            PageType = pageType;
            if (!string.IsNullOrEmpty(content)) Content = content;
            if (!string.IsNullOrEmpty(url)) Url = url;
            return this;
        }
    }
}