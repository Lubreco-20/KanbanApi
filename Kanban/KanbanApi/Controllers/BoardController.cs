using Microsoft.AspNetCore.Mvc;
using KanbanApi.DTOs;
using KanbanApi.Interfaces;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoard(int id)
        {
            var board = await _boardService.GetBoardByIdAsync(id);

            return board is null ? NotFound() : Ok(board);
        }

        /// <summary>
        /// Gets all boards associated with a specific user by their user ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetBoardByUserId(int userId)
        {
            var board = await _boardService.GetBoardsByUserIdAsync(userId);

            return Ok(board);
        }

        /// <summary>
        /// Creates a new board for a specific user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateBoard(int userId, CreateBoardRequest request)
        {
            var board = await _boardService.CreateBoardAsync(userId, request);

            return CreatedAtAction(nameof(GetBoard), new { id = board.Id }, board);
        }

        /// <summary>
        /// Updates an existing board by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoard(int id, UpdateBoardRequest request)
        {
            var board = await _boardService.UpdateBoardAsync(id, request);

            return board is null ? NotFound() : Ok(board);
        }

        /// <summary>
        /// Deletes an existing board by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            var deleted = await _boardService.DeleteBoardAsync(id);

            return deleted ? NoContent() : NotFound();
        }
    }
}
