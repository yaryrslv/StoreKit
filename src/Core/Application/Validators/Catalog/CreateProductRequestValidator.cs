using System;
using StoreKit.Application.Validators.General;
using StoreKit.Shared.DTOs.Catalog;
using FluentValidation;

namespace StoreKit.Application.Validators.Catalog
{
    public class CreateProductRequestValidator : CustomValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
            RuleFor(p => p.CategoryId).NotEqual(Guid.Empty);
            RuleFor(p => p.Image).SetValidator(new FileUploadRequestValidator());
            RuleFor(p => p.Tags).NotEmpty().NotNull();
        }
    }
}