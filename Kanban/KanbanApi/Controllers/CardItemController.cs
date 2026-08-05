using Microsoft.AspNetCore.Mvc;
using KanbanApi.DTOs;
using KanbanApi.Interfaces;

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
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCardItem(int id)
        {
            var cardItem = await _cardItemService.GetCardItemByIdAsync(id);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Gets all card items associated with a specific board column by its ID.
        /// </summary>
        /// <param name="boardColumnId"></param>
        /// <returns></returns>
        [HttpGet("Column/{boardColumnId}")]
        public async Task<IActionResult> GetCardItemsByBoardColumnId(int boardColumnId)
        {
            var cardItems = await _cardItemService.GetCardItemsByColumnIdAsync(boardColumnId);
            return Ok(cardItems);
        }

        /// <summary>
        /// Creates a new card item in a specific board column.
        /// </summary>
        /// <param name="boardColumnId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCardItem(int boardColumnId, CreateCardItemRequest request)
        {
            var cardItem = await _cardItemService.CreateCardItemAsync(boardColumnId, request);
            return CreatedAtAction(nameof(GetCardItem), new { id = cardItem.Id }, cardItem);
        }

        /// <summary>
        /// Updates an existing card item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCardItem(int id, UpdateCardItemRequest request)
        {
            var cardItem = await _cardItemService.UpdateCardItemAsync(id, request);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Changes the order of a card item within its board column.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{id}/order")]
        public async Task<IActionResult> ChangeOrderCardItem(int id, ChangeOrderCardItemRequest request)
        {
            var cardItem = await _cardItemService.ChangeOrderCardItemAsync(id, request);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Moves a card item to a different board column.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{id}/move")]
        public async Task<IActionResult> MoveCardItemColumn(int id, MoveCardItemColumnRequest request)
        {
            var cardItem = await _cardItemService.MoveCardItemColumnAsync(id, request);
            return cardItem is null ? NotFound() : Ok(cardItem);
        }

        /// <summary>
        /// Deletes a card item by its ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardItem(int id)
        {
            var success = await _cardItemService.DeleteCardItemAsync(id);
            return success ? NoContent() : NotFound();
        }
    }
}
