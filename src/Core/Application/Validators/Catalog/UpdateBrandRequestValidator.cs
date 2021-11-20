using StoreKit.Shared.DTOs.Catalog;
using FluentValidation;

namespace StoreKit.Application.Validators.Catalog
{
    public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
    {
        public UpdateBrandRequestValidator()
        {
            RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
        }
    }
}