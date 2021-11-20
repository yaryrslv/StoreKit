using StoreKit.Application.Wrapper;
using StoreKit.Shared.DTOs.Identity.Requests;
using StoreKit.Shared.DTOs.Identity.Responses;
using System.Threading.Tasks;

namespace StoreKit.Application.Abstractions.Services.Identity
{
    public interface ITokenService : ITransientService
    {
        Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress);

        Task<IResult<TokenResponse>> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
    }
}