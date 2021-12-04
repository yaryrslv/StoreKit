using StoreKit.Application.Validators.General;
using StoreKit.Shared.DTOs.Catalog;
using FluentValidation;

namespace StoreKit.Application.Validators.Catalog
{
    public class UpdateProductRequestValidator : CustomValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(p => p.Name).MaximumLength(75).NotEmpty();
            RuleFor(p => p.Rate).GreaterThanOrEqualTo(1).NotEqual(0);
            RuleFor(p => p.Image).SetValidator(new FileUploadRequestValidator());
            RuleFor(p => p.Tags).NotEmpty().NotNull();
        }
    }
}