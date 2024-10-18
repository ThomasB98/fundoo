using BusinessLayer.Service;
using DataLayer.Constants.ResponeEntity;
using DataLayer.Utilities.Emial;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.DTO;

namespace fundoo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        
        [HttpPost("Login")]
        [AllowAnonymous]
        public Task<ResponseBody<string>> AuthenticateAsync([FromBody]LoginDto login)
        {
            return _authService.AuthenticateAsync(login);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailDto request)
        {
            var emailDto = new EmailDto
            {
                To = request.To,
            };

            bool result = await _emailService.SendEmailAsync(emailDto);

            if (result)
            {
                return Ok("Password reset link has been sent to your email");
            }

            return BadRequest("Failed to send password reset email");
        }
    }
}
