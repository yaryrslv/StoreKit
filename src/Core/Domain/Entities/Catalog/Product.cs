using StoreKit.Domain.Contracts;
using StoreKit.Domain.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreKit.Domain.Entities.Catalog
{
    public class Product : AuditableEntity, IMustHaveTenant
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string TenantKey { get; set; }
        public string ImagePath { get; set; }
        [Column(TypeName = "jsonb")]
        public List<Tag> Tags { get; set; }

        public Product(string name, string description, string imagePath, List<Tag> tags)
        {
            Name = name;
            Description = description;
            ImagePath = imagePath;
            Tags = tags;
        }

        public Product Update(string name, string description, decimal rate, string imagePath, List<Tag> tags)
        {
            if (name != null && !Name.NullToString().Equals(name)) Name = name;
            if (description != null && !Description.NullToString().Equals(description)) Description = description;
            if (imagePath != null && !ImagePath.NullToString().Equals(imagePath)) ImagePath = imagePath;
            if (tags != null && !ImagePath.NullToString().Equals(tags)) Tags = tags;
            return this;
        }
    }
}