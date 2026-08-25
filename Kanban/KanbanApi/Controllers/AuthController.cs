using KanbanApi.Services;
using KanbanApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using KanbanApi.Interfaces;

namespace KanbanApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService service) => authService = service;

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await authService.LoginAsync(request);
            return result is null ? Unauthorized("Invalid credentials") : Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            var succes = await authService.LogoutAsync(request);
            return succes ? NoContent() : NotFound();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var result = await authService.RefreshAsync(request);
            return result is null ? Unauthorized("Invalid or expired token") : Ok(result);
        }
    }
}
