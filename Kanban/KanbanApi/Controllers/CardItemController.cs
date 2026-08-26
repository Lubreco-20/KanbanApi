using KanbanApi.DTOs;
using KanbanApi.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KanbanApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardItemController : ControllerBase
    {
        private readonly ICardItemService _cardItemService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardItemController"/> class.
        /// </summary>
        /// <param name="cardItemService"></param>
        public CardItemController(ICardItemService cardItemService) => _cardItemService = cardItemService;

        /// <summary>
        /// Gets a card item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardItem(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var cardItem = await _cardItemService.GetCardItemByIdAsync(id, userId);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Gets all card items associated with a specific board column by its ID.
        /// </summary>
        /// <param name="boardColumnId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Column/{boardColumnId}")]
        public async Task<IActionResult> GetCardItemsByBoardColumnId(int boardColumnId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var cardItems = await _cardItemService.GetCardItemsByColumnIdAsync(boardColumnId, userId);
            return Ok(cardItems);
        }

        /// <summary>
        /// Creates a new card item in a specific board column.
        /// </summary>
        /// <param name="boardColumnId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCardItem(int boardColumnId, CreateCardItemRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var cardItem = await _cardItemService.CreateCardItemAsync(boardColumnId, userId, request);
            return CreatedAtAction(nameof(GetCardItem), new { id = cardItem!.Id }, cardItem);
        }

        /// <summary>
        /// Updates an existing card item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCardItem(int id, UpdateCardItemRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var cardItem = await _cardItemService.UpdateCardItemAsync(id, userId, request);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Changes the order of a card item within its board column.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("{id}/order")]
        public async Task<IActionResult> ChangeOrderCardItem(int id, ChangeOrderCardItemRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var cardItem = await _cardItemService.ChangeOrderCardItemAsync(id, userId, request);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Moves a card item to a different board column.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("{id}/move")]
        public async Task<IActionResult> MoveCardItemColumn(int id, MoveCardItemColumnRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var cardItem = await _cardItemService.MoveCardItemColumnAsync(id, userId, request);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Deletes a card item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardItem(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = int.Parse(userIdClaim!);

            var success = await _cardItemService.DeleteCardItemAsync(id, userId);
            return success ? NoContent() : NotFound();
        }
    }
}
