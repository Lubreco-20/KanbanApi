using KanbanApi.DTOs;
using KanbanApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KanbanApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardService _boardService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardController"/> class.
        /// </summary>
        /// <param name="boardService"></param>
        public BoardController(IBoardService boardService) => _boardService = boardService;

        /// <summary>
        /// Gets all boards.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoard(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var board = await _boardService.GetBoardByIdAsync(id, userId);

            return board is null ? NotFound() : Ok(board);
        }

        /// <summary>
        /// Gets all boards associated with a specific user by their user ID.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyBoards()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var board = await _boardService.GetBoardsByUserIdAsync(userId);

            return Ok(board);
        }

        /// <summary>
        /// Creates a new board for a specific user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBoard(CreateBoardRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var board = await _boardService.CreateBoardAsync(userId, request);

            return CreatedAtAction(nameof(GetBoard), new { id = board!.Id }, board);
        }

        /// <summary>
        /// Updates an existing board by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(int id, UpdateBoardRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var board = await _boardService.UpdateBoardAsync(id, userId, request);

            return board is null ? NotFound() : Ok(board);
        }

        /// <summary>
        /// Deletes an existing board by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var deleted = await _boardService.DeleteBoardAsync(id, userId);

            return deleted ? NoContent() : NotFound();
        }
    }
}
