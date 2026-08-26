using KanbanApi.DTOs;
using KanbanApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KanbanApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BoardColumnController : ControllerBase
    {
        private readonly IBoardColumnService _boardColumnService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoardColumnController"/> class.
        /// </summary>
        /// <param name="boardColumnService"></param>
        public BoardColumnController(IBoardColumnService boardColumnService) => _boardColumnService = boardColumnService;

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoardColumn(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var column = await _boardColumnService.GetBoardColumnByIdAsync(id, userId);

            return column is null ? NotFound() : Ok(column);
        }

        /// <summary>
        /// Gets all board columns associated with a specific board by its ID.
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Board/{boardId}")]
        public async Task<IActionResult> GetBoardColumnsByBoardId(int boardId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var columns = await _boardColumnService.GetBoardColumnsByBoardIdAsync(boardId, userId);
            
            return Ok(columns);
        }

        /// <summary>
        /// Creates a new board column for a specific board.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateBoardColumn(int boardId, CreateBoardColumnRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var column = await _boardColumnService.CreateBoardColumnAsync(boardId, userId, request);
            
            return CreatedAtAction(nameof(GetBoardColumn), new { id = column!.Id }, column);
        }

        /// <summary>
        /// Updates a board column by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoardColumn(int id, UpdateBoardColumnRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var column = await _boardColumnService.UpdateBoardColumnAsync(id, userId, request);

            return column is null ? NotFound() : Ok(column);
        }

        /// <summary>
        /// Deletes a board column by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoardColumn(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var deleted = await _boardColumnService.DeleteBoardColumnAsync(id, userId);

            return deleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Moves a board column to a new order position.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("{id}/order")]
        public async Task<IActionResult> ChangeOrderBoardColumn(int id, ChangeOrderBoardColumnRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var column = await _boardColumnService.ChangeOrderBoardColumnAsync(id, userId, request);

            return column is null ? NotFound() : Ok(column);
        }
    }
}
