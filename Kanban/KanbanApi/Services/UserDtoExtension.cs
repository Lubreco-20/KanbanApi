using KanbanApi.DTOs;
using KanbanApi.Models;

namespace KanbanApi.Services
{
    /// <summary>
    /// Extension methods for converting User model to UserResponse DTO.
    /// </summary>
    public static class UserDtoExtension
    {
        /// <summary>
        /// Converts a User model to a UserResponse DTO.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static UserResponse ToDto(this User user)
        {
            return new UserResponse(
                user.UserId,
                user.Username,
                user.Email
            );
        }
    }
}