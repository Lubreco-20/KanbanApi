using KanbanApi.Data;
using KanbanApi.DTOs;
using KanbanApi.Models;
using BCrypt.Net;
using KanbanApi.Interfaces;

namespace KanbanApi.Services
{
    public class UserService : IUserService
    {
        private readonly KanbanDbContext context;

        /// <summary>
        /// Initializes a new instance of the UserService class with the specified database context.
        /// </summary>
        /// <param name="context"></param>
        public UserService(KanbanDbContext context) => this.context = context;

        /// <summary>
        /// Creates a new user in the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
        {
            // Save the user in the database
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password) // Hash the password
            };

            // Hash the password before storing it
            context.Users.Add(user);
            await context.SaveChangesAsync();

            // Return the created user as a DTO
            return user.ToDto();
        }

        /// <summary>
        /// Updates an existing user in the database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            // Find the user by ID
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return null;

            // Update the user's properties
            user.Username = request.Username;
            user.Email = request.Email;

            // If the password is provided, update it
            await context.SaveChangesAsync();
            return user.ToDto();
        }

        public async Task<UserResponse?> UpdatePasswordAsync(int id, UpdatePasswordRequest request)
        {
            // Find the user by ID
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return null;

            // Verify the old password
            if (!BCrypt.Net.BCrypt.Verify(request.OldPassword, user.PasswordHash))
                return null;

            // Update the user's password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            await context.SaveChangesAsync();
            return user.ToDto();
        }

        /// <summary>
        /// Gets a user by their ID from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            // Find the user by ID
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return null;

            // Return the user as a DTO
            return user.ToDto();
        }
        
        /// <summary>
        /// Deletes a user by their ID from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUserAsync(int id)
        {
            // Find the user by ID
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return false;

            // Remove the user from the database
            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
