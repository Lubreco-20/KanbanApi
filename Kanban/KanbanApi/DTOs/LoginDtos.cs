namespace KanbanApi.DTOs
{
    public sealed record LoginRequest(
        string Email,
        string Password
        );

    public sealed record LogoutRequest(
        string refreshToken);

    public sealed record RefreshRequest(
        string RefreshToken);

    public sealed record LoginResponse(
        string AccesToken,
        string RefreshToken,
        UserResponse User
        );
}
