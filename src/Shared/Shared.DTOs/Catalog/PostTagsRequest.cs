using System;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class PostTagsRequest : IMustBeValid
    {
        public Guid ProductId { get; set; }
        public TagType TagType { get; set; }
    }
}