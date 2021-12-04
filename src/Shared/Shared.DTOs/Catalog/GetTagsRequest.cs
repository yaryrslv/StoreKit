using System;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class GetTagsRequest : IMustBeValid
    {
        public Guid ProductId { get; set; }
    }
}