using System;

namespace StoreKit.Shared.DTOs.Identity.Responses
{
    public record TokenResponse(string Token, string RefreshToken, DateTime RefreshTokenExpiryTime);
}