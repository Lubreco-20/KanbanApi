namespace KanbanApi.DTOs
{
    public sealed record CreateUserRequest(
        string Username,
        string Email,
        string Password
        );

    public sealed record UpdateUserRequest(
        string Username,
        string Email
        );

    public sealed record UpdatePasswordRequest(
        string OldPassword,
        string NewPassword
        );

    public sealed record UserResponse(
        int Id,
        string Username,
        string Email
        );
}
