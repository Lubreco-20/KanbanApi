using Microsoft.AspNetCore.Mvc;
using KanbanApi.DTOs;
using KanbanApi.Interfaces;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBoardColumn(int id)
        {
            var column = await _boardColumnService.GetBoardColumnByIdAsync(id);

            return column is null ? NotFound() : Ok(column);
        }

        /// <summary>
        /// Gets all board columns associated with a specific board by its ID.
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        [HttpGet("Board/{BoardId}")]
        public async Task<IActionResult> GetBoardColumnsByBoardId(int boardId)
        {
            var columns = await _boardColumnService.GetBoardColumnsByBoardIdAsync(boardId);
            
            return Ok(columns);
        }

        /// <summary>
        /// Creates a new board column for a specific board.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateBoardColumn(int boardId, CreateBoardColumnRequest request)
        {
            var column = await _boardColumnService.CreateBoardColumnAsync(boardId, request);
            
            return CreatedAtAction(nameof(GetBoardColumn), new { id = column.Id }, column);
        }

        /// <summary>
        /// Updates a board column by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBoardColumn(int id, UpdateBoardColumnRequest request)
        {
            var column = await _boardColumnService.UpdateBoardColumnAsync(id, request);

            return column is null ? NotFound() : Ok(column);
        }

        /// <summary>
        /// Deletes a board column by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoardColumn(int id)
        {
            var deleted = await _boardColumnService.DeleteBoardColumnAsync(id);

            return deleted ? NoContent() : NotFound();
        }
    }
}
