using StoreKit.Domain.Contracts;

namespace StoreKit.Domain.Entities.Catalog
{
    public class StaticPage : AuditableEntity
    {
        public string Body { get; set; }
        public StaticPage(string body)
        {
            Body = body;
        }
        public StaticPage Update(string body)
        {
            if (!string.IsNullOrEmpty(body)) Body = body;
            return this;
        }
    }
}