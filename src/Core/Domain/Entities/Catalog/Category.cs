using System.Collections;
using System.Collections.Generic;
using StoreKit.Domain.Contracts;

namespace StoreKit.Domain.Entities.Catalog
{
    public class Category : AuditableEntity, IMustHaveTenant
    {
        public string Name { get; set; }
        public string TenantKey { get; set; }
        public ICollection<Product> Product { get; set; }
        public Category(string name)
        {
            Name = name;
        }

        public Category Update(string name)
        {
            if (name != null) Name = name;
            return this;
        }
    }
}