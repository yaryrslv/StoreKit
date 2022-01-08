using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.General.Requests;

namespace StoreKit.Shared.DTOs.Catalog.Product
{
    public class UpdateProductRequest : IMustBeValid
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid CategoryId { get; set; }
        public FileUploadRequest Image { get; set; }
        [Column(TypeName = "jsonb")]
        public List<Tag> Tags { get; set; }
    }
}