using BusinessLayer.Service;
using DataLayer.Constants.ResponeEntity;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        
        [HttpPost("Login")]
        [AllowAnonymous]
        public Task<ResponseBody<string>> AuthenticateAsync([FromBody]LoginDto login)
        {
            return _authService.AuthenticateAsync(login);
        }
    }
}
