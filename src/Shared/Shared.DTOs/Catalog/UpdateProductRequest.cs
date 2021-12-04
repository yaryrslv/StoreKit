using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using StoreKit.Domain.Entities.Catalog;
using StoreKit.Shared.DTOs.General.Requests;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class UpdateProductRequest : IMustBeValid
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        [Column(TypeName = "jsonb")]
        public TagType Tags { get; set; }
        public FileUploadRequest Image { get; set; }
    }
}