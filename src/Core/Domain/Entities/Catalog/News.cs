using System;
using StoreKit.Domain.Contracts;
using StoreKit.Domain.Extensions;

namespace StoreKit.Domain.Entities.Catalog
{
    public class News : AuditableEntity, IMustHaveTenant
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime CreationTime { get; private set; }
        public string TenantKey { get; set; }

        public News(string title, string description)
        {
            Title = title;
            Description = description;
            CreationTime = DateTime.Now;
        }

        public News Update(string name, string description)
        {
            if (name != null && !Title.NullToString().Equals(name)) Title = name;
            if (description != null && !Description.NullToString().Equals(description)) Description = description;
            return this;
        }
    }
}