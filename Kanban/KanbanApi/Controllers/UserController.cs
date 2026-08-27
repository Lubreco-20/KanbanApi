using KanbanApi.DTOs;
using KanbanApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KanbanApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserController"/> class.
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Gets a user by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int authenticatedUserId = int.Parse(userIdClaim!);

            if (id != authenticatedUserId)
                return NotFound();

            // Retrieve the user by ID using the user service
            var user = await _userService.GetUserByIdAsync(id, authenticatedUserId);

            // If the user is not found, return a 404 Not Found response; otherwise, return the user data with a 200 OK response
            return user is null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserRequest request)
        {
            // Create a new user using the user service
            var user = await _userService.CreateUserAsync(request);

            // Return a 201 Created response with the location of the newly created user
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user by their ID.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(UpdateUserRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int id = int.Parse(userIdClaim!);

            // Update the user using the user service
            var user = await _userService.UpdateUserAsync(id, request);

            // If the user is not found, return a 404 Not Found response; otherwise, return the updated user data with a 200 OK response
            return user is null ? NotFound() : Ok(user);
        }

        /// <summary>
        /// Updates the password of an existing user by their ID.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}/password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int id = int.Parse(userIdClaim!);

            // Update the user's password using the user service
            var user = await _userService.UpdatePasswordAsync(id, request);

            // If the user is not found or the current password is invalid, return a 400 Bad Request response; otherwise, return the updated user data with a 200 OK response
            return user is null ? BadRequest("Invalid user or current password") : Ok(user);
        }

        /// <summary>
        /// Deletes a user by their ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int authenticatedUserId = int.Parse(userIdClaim!);

            if (id != authenticatedUserId)
                return NotFound();

            // Delete the user using the user service
            var deleted = await _userService.DeleteUserAsync(id, authenticatedUserId);

            // If the user is not found, return a 404 Not Found response; otherwise, return a 204 No Content response indicating successful deletion
            return deleted ? NoContent() : NotFound();
        }
    }
}
