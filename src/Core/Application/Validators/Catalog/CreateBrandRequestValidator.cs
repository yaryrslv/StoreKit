using StoreKit.Shared.DTOs.Catalog;
using FluentValidation;

namespace StoreKit.Application.Validators.Catalog
{
    public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
    {
        public CreateBrandRequestValidator()
        {
            RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
        }
    }
}