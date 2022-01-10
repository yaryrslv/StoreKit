using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog.Product
{
    public class CreateProductRequest : IMustBeValid
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        [Column(TypeName = "jsonb")]
        public List<Tag> Tags { get; set; }
    }
}