using StoreKit.Domain.Contracts;

namespace StoreKit.Domain.Entities.Catalog;

public class StaticPage : AuditableEntity, IMustHaveTenant
{
    public string TenantKey { get; set; }
    public string Body { get; set; }
}