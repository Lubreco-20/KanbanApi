using System.ComponentModel.DataAnnotations;

namespace KanbanApi.DTOs
{
    public sealed record LoginRequest(
        [Required(ErrorMessage = "An email is required")]
        [EmailAddress(ErrorMessage = "The email address entered is invalid")]
        string Email,

        [Required(ErrorMessage = "A password is required")]
        [MinLength(8, ErrorMessage = "The password must have at least 8 characters")]
        string Password
        );

    public sealed record LogoutRequest(
        [Required(ErrorMessage = "A refresh token is required")]
        string RefreshToken);

    public sealed record RefreshRequest(
        [Required(ErrorMessage = "A refresh token is required")]
        string RefreshToken);

    public sealed record LoginResponse(
        string AccesToken,
        string RefreshToken,
        UserResponse User
        );
}
