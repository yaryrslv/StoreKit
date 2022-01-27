using System;
using System.Collections.Generic;
using StoreKit.Domain.Contracts;
using StoreKit.Domain.Extensions;

namespace StoreKit.Domain.Entities.Catalog
{
    public class ProductInBasket
    {
        public Guid Id { get; set; }
        public string Name { get;set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string PriceType { get; set; }
        public decimal Price{ get; set; }

        public ProductInBasket(string name, string description, string imagePath, string priceType, decimal price)
        {
            Name = name;
            Description = description;
            ImagePath = imagePath;
            PriceType = priceType;
            Price= price;
        }

        public ProductInBasket Update(string name, string description, string imagePath,string priceType, decimal price)
        {
            if (name != null && !Name.NullToString().Equals(name)) Name = name;
            if (description != null) Description = description;
            if (imagePath != null && !ImagePath.NullToString().Equals(imagePath)) ImagePath = imagePath;
            if (priceType != null && !Name.NullToString().Equals(priceType)) PriceType = priceType;
            if (price > 0) Price = price;
            return this;
        }
    }
}