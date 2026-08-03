using KanbanApi.Data;
using KanbanApi.DTOs;
using KanbanApi.Models;
using BCrypt.Net;

namespace KanbanApi.Services
{
    public class UserService
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
        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUserRequest request)
        {
            // Find the user by ID
            var user = await context.Users.FindAsync(id);
            if (user == null)
                throw new InvalidOperationException("User not found");

            // Update the user's properties
            user.Username = request.Username;
            user.Email = request.Email;

            // If the password is provided, update it
            await context.SaveChangesAsync();
            return user.ToDto();
        }

        public async Task<UserResponse> UpdatePasswordAsync(UpdatePasswordRequest request)
        {
            // Implementation for updating user password
            throw new NotImplementedException();
        }
    }
}
