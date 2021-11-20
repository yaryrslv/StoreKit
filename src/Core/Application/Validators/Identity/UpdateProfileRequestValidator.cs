using StoreKit.Application.Validators.General;
using StoreKit.Shared.DTOs.Identity.Requests;
using FluentValidation;

namespace StoreKit.Application.Validators.Identity
{
    public class UpdateProfileRequestValidator : CustomValidator<UpdateProfileRequest>
    {
        public UpdateProfileRequestValidator()
        {
            RuleFor(p => p.FirstName).MaximumLength(75).NotEmpty();
            RuleFor(p => p.LastName).MaximumLength(75).NotEmpty();
            RuleFor(p => p.Email).NotEmpty();
            RuleFor(p => p.Image).SetValidator(new FileUploadRequestValidator());
        }
    }
}