using System.ComponentModel.DataAnnotations;

namespace KanbanApi.DTOs
{
    public sealed record CreateUserRequest(
        [Required(ErrorMessage = "A username is required")]
        [MinLength(4, ErrorMessage = "The user name must have at least 6 characters")]
        string Username,

        [Required(ErrorMessage = "An email is required")]
        [EmailAddress(ErrorMessage = "The email address entered is invalid")]
        string Email,

        [Required(ErrorMessage = "A password is required")]
        [MinLength(8, ErrorMessage = "The password must have at least 8 characters")]
        string Password
        );

    public sealed record UpdateUserRequest(
        [Required(ErrorMessage = "A username is required")]
        [MinLength(6, ErrorMessage = "The user name must have at least 6 characters")]
        string Username,

        [Required(ErrorMessage = "An email is required")]
        [EmailAddress(ErrorMessage = "The email address entered is invalid")]
        string Email
        );

    public sealed record UpdatePasswordRequest(
        [Required(ErrorMessage = "The old password is required")]
        [MinLength(8, ErrorMessage = "The password must have at least 8 characters")]
        string OldPassword,

        [Required(ErrorMessage = "A new password is required")]
        [MinLength(8, ErrorMessage = "The password must have at least 8 characters")]
        string NewPassword
        );

    public sealed record UserResponse(
        int Id,
        string Username,
        string Email
        );
}
