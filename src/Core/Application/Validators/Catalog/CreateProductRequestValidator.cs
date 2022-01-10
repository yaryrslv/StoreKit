using System;
using StoreKit.Application.Validators.General;
using StoreKit.Shared.DTOs.Catalog;
using FluentValidation;
using StoreKit.Shared.DTOs.Catalog.Product;

namespace StoreKit.Application.Validators.Catalog
{
    public class CreateProductRequestValidator : CustomValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
            RuleFor(p => p.CategoryId).NotEqual(Guid.Empty);
            RuleFor(p => p.Tags).NotEmpty().NotNull();
        }
    }
}