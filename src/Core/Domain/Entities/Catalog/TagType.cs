using System.Collections.Generic;

namespace StoreKit.Domain.Entities.Catalog
{
    public class TagType
    {
        public string Name { get; set; }
        public List<Tag> Tags { get; set; }
    }
}