using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog.Product
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        [Column(TypeName = "jsonb")]
        public List<Tag> Tags { get; set; }

        public List<ProductPrice> Prices { get; set; }
    }
}