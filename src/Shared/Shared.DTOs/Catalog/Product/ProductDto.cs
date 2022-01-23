using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog.Product
{
    public class ProductDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public Guid CategoryId { get; set; }
        [Column(TypeName = "jsonb")]
        public List<Tag> Tags { get; set; }
        [Column(TypeName = "jsonb")]
        public List<ProductPrice> Prices { get; set; }
    }
}