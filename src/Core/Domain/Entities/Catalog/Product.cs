using StoreKit.Domain.Contracts;
using StoreKit.Domain.Extensions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace StoreKit.Domain.Entities.Catalog
{
    public class Product : AuditableEntity, IMustHaveTenant
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Rate { get; private set; }
        public string TenantKey { get; set; }
        public string ImagePath { get; set; }
        [Column(TypeName = "jsonb")]
        public TagType TagType { get; set; }

        public Product(string name, string description, decimal rate, TagType tags, string imagePath)
        {
            Name = name;
            Description = description;
            Rate = rate;
            ImagePath = imagePath;
            TagType = tags;
        }

        protected Product()
        {
        }

        public Product Update(string name, string description, decimal rate, TagType tags, string imagePath)
        {
            if (name != null && !Name.NullToString().Equals(name)) Name = name;
            if (description != null && !Description.NullToString().Equals(description)) Description = description;
            if (Rate != rate) Rate = rate;
            TagType = tags;
            if (imagePath != null && !ImagePath.NullToString().Equals(imagePath)) ImagePath = imagePath;
            return this;
        }
    }
}