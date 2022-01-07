using System;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class GetTagTypeRequest : IMustBeValid
    {
        public Guid ProductId { get; set; }
    }
}