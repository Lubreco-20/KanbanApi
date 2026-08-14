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

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshRequest request)
        {
            var result = await authService.RefreshAsync(request);
            return result is null ? Unauthorized("Invalid or expired token") : Ok(result);
        }
    }
}
