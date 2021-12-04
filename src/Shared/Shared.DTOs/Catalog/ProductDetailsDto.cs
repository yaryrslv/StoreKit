using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class ProductDetailsDto : IDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public string ImagePath { get; set; }
        [Column(TypeName = "jsonb")]
        public TagType TagType { get; set; }
    }
}