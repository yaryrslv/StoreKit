using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Identity.Requests;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.Identity
{
    public interface IIdentityService : ITransientService
    {
        Task<IResult> RegisterAsync(RegisterRequest request, string origin);

        Task<IResult<string>> ConfirmEmailAsync(string userId, string code, string tenantKey);

        Task<IResult<string>> ConfirmPhoneNumberAsync(string userId, string code);

        Task<IResult> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);

        Task<IResult> ResetPasswordAsync(ResetPasswordRequest request);
        Task<IResult> UpdateProfileAsync(UpdateProfileRequest request, string userId);
    }
}