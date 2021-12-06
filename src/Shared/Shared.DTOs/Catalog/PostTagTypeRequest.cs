using System;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class PostTagTypeRequest : IMustBeValid
    {
        public Guid ProductId { get; set; }
        public TagType TagType { get; set; }
    }
}