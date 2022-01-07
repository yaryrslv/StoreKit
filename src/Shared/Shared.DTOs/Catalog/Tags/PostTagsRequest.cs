using System.Collections.Generic;
using StoreKit.Domain.Entities.Catalog;

namespace StoreKit.Shared.DTOs.Catalog
{
    public class PostTagsRequest : IMustBeValid
    {
        public string TagName { get; set; }
        public List<Tag> Tags { get; set; }
    }
}