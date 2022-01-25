using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using StoreKit.Domain.Contracts;

namespace StoreKit.Domain.Entities.Catalog
{
    public class Basket : BaseEntity
    {
        public Guid UserId { get; set; }
        [Column(TypeName = "jsonb")]
        public List<Product> Products { get; set; }
        public Basket(Guid userId, List<Product> products)
        {
            UserId = userId;
            Products = products;
        }

        public Basket Update(Guid userId, List<Product> products)
        {
            if (userId != Guid.Empty) UserId = userId;
            if (products != null && products.Count > 0 && products != Products) Products = products;
            return this;
        }
    }
}