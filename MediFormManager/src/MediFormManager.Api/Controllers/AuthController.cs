using MediFormManager.Application.DTOs.Auth;
using MediFormManager.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MediFormManager.Api.Controllers
{    
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService) {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);
            if (result is null) { return Unauthorized(); }
            return Ok(result);
        }
    }
}
