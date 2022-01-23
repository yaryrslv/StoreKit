using System.ComponentModel.DataAnnotations;
using StoreKit.Application.Abstractions.Services.Identity;
using StoreKit.Domain.Constants;
using StoreKit.Infrastructure.Identity.Permissions;
using StoreKit.Shared.DTOs.Identity.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using StoreKit.Infrastructure.SwaggerFilters;

namespace StoreKit.Bootstrapper.Controllers.Identity
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class IdentityController : ControllerBase
    {
        private readonly ICurrentUser _user;
        private readonly IIdentityService _identityService;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public IdentityController(IIdentityService identityService, ICurrentUser user, IUserService userService, ITokenService tokenService)
        {
            _identityService = identityService;
            _user = user;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [SwaggerHeader("tenantKey", "Input your tenant Id to access this API", "", true)]
        public async Task<IActionResult> RegisterAsync(RegisterRequest request, [FromHeader(Name = "tenantKey")][Required] string tenantKey = null)
        {
            string baseUrl = $"{this.Request.Scheme}://{this.Request.Host.Value.ToString()}{this.Request.PathBase.Value.ToString()}";
            string origin = string.IsNullOrEmpty(Request.Headers["origin"].ToString()) ? baseUrl : Request.Headers["origin"].ToString();
            var identityResponse = await _identityService.RegisterAsync(request, origin);
            var tokenResponse = await _tokenService.GetTokenAsync(new TokenRequest(request.Email, request.Password), GenerateIPAddress());
            return Ok(tokenResponse);
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string userId, [FromQuery] string code, [FromQuery] string tenantKey)
        {
            return Ok(await _identityService.ConfirmEmailAsync(userId, code, tenantKey));
        }

        [HttpGet("confirm-phone-number")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmPhoneNumberAsync([FromQuery] string userId, [FromQuery] string code)
        {
            return Ok(await _identityService.ConfirmPhoneNumberAsync(userId, code));
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            var origin = Request.Headers["origin"];
            return Ok(await _identityService.ForgotPasswordAsync(request, origin));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordRequest request)
        {
            return Ok(await _identityService.ResetPasswordAsync(request));
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfileAsync(UpdateProfileRequest request)
        {
            return Ok(await _identityService.UpdateProfileAsync(request, _user.GetUserId().ToString()));
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileDetailsAsync()
        {
            return Ok(await _userService.GetAsync(_user.GetUserId().ToString()));
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();
            }
        }
    }
}