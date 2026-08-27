using KanbanApi.Data;
using KanbanApi.DTOs;
using KanbanApi.Models;
using Microsoft.EntityFrameworkCore;
using KanbanApi.Interfaces;

namespace KanbanApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly KanbanDbContext context;
        private readonly ITokenService service;

        public AuthService(KanbanDbContext context, ITokenService service)
        {
            this.context = context;
            this.service = service;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            var accessToken = service.GenerateAccessToken(user);
            var refreshTokenValue = service.GenerateRefreshToken();

            var refreshToken = new RefreshToken
            {
                Token = refreshTokenValue,
                ExpiredDate = DateTime.UtcNow.AddDays(7),
                IsRevoked = false,
                UserId = user.UserId
            };

            context.Tokens.Add(refreshToken);
            await context.SaveChangesAsync();

            return new LoginResponse(accessToken, refreshTokenValue, user.ToDto());
        }

        public async Task<bool> LogoutAsync(LogoutRequest request)
        {
            var storedToken = await context.Tokens
                .FirstOrDefaultAsync(t => t.Token == request.RefreshToken);

            if (storedToken == null)
                return false;

            storedToken.IsRevoked = true;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<LoginResponse?> RefreshAsync(RefreshRequest request)
        {
            var storedToken = await context.Tokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiredDate <= DateTime.UtcNow)
                return null;

            storedToken.IsRevoked = true;

            var newAccessToken = service.GenerateAccessToken(storedToken.User);
            var newRefreshTokenValue = service.GenerateRefreshToken();

            var newRefreshToken = new RefreshToken
            {
                Token = newRefreshTokenValue,
                IsRevoked = false,
                ExpiredDate = DateTime.UtcNow.AddDays(7),
                UserId = storedToken.UserId
            };

            context.Tokens.Add(newRefreshToken);
            await context.SaveChangesAsync();

            return new LoginResponse(newAccessToken, newRefreshTokenValue, storedToken.User.ToDto());
        }
    }
}
