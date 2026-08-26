using KanbanApi.Data;
using KanbanApi.DTOs;
using KanbanApi.Interfaces;
using KanbanApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KanbanApi.Services
{
    public class CardItemService : ICardItemService
    {
        private readonly KanbanDbContext context;

        /// <summary>
        /// Initializes a new instance of the CardItemService class with the specified database context.
        /// </summary>
        /// <param name="context"></param>
        public CardItemService(KanbanDbContext context) => this.context = context;

        /// <summary>
        /// Creates a new card item in the database based on the provided request and returns the created card item as a response DTO.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CardItemResponse?> CreateCardItemAsync(int columnId, int userId, CreateCardItemRequest request)
        {
            // Check if desired column exist
            var column = await context.BoardColumns
                .Include(bc => bc.Board)
                .FirstOrDefaultAsync(bc => bc.ColumnId == columnId);
            if (column == null || column.Board.UserId != userId) 
                return null;

            // Look if there is any card in the column
            var cardsInColumn = await context.CardItems.
                Where(ci => ci.ColumnId == columnId).
                ToListAsync();

            // If there is no card, then this card will take the Order 0, otherwise, the card will be created at the bottom
            int newOrder = cardsInColumn.Any() ? cardsInColumn.Max(c => c.Order) + 1 : 0;

            // Create a new CardItem entity based on the request data
            var cardItem = new CardItem
            {
                Title = request.Title,
                Description = request.Description,
                ColumnId = columnId,
                Order = newOrder,
            };

            // Add the new card item to the database context and save changes
            context.CardItems.Add(cardItem);
            await context.SaveChangesAsync();

            // Return the created card item as a response DTO
            return cardItem.ToDto();
        }

        /// <summary>
        /// Updates an existing card item in the database based on the provided ID and request data, and returns the updated card item as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<CardItemResponse?> UpdateCardItemAsync(int id, int userId, UpdateCardItemRequest request)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems
                .Include(cd => cd.Column)
                .ThenInclude(bc => bc.Board)
                .FirstOrDefaultAsync(cd => cd.CardId == id);
            if (cardItem == null || cardItem.Column.Board.UserId != userId) 
                return null;

            // Update the card item properties based on the request data
            cardItem.Title = request.Title;
            cardItem.Description = request.Description;

            // Save changes to the database
            await context.SaveChangesAsync();
            return cardItem.ToDto();
        }

        /// <summary>
        /// Retrieves a card item from the database by its ID and returns it as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<CardItemResponse?> GetCardItemByIdAsync(int id, int userId)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems
                .Include(cd => cd.Column)
                .ThenInclude(bc => bc.Board)
                .FirstOrDefaultAsync(cd => cd.CardId == id);
            if (cardItem == null || cardItem.Column.Board.UserId != userId)
                return null;

            // Return the found card item as a response DTO
            return cardItem.ToDto();
        }

        /// <summary>
        /// Retrieves all card items associated with a specific column ID from the database and returns them as a collection of response DTOs.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CardItemResponse>> GetCardItemsByColumnIdAsync(int columnId, int userId)
        {
            // Retrieve all card items associated with the specified column ID
            var cardItems = await context.CardItems
                .Include(cd => cd.Column)
                .ThenInclude(bc => bc.Board)
                .Where(ci => ci.ColumnId == columnId && ci.Column.Board.UserId == userId)
                .ToListAsync();

            // Convert the list of card items to a list of response DTOs and return it
            return cardItems.Select(ci => ci.ToDto());
        }

        /// <summary>
        /// Changes the order of a card item in the database based on the provided ID and request data, and returns the updated card item as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CardItemResponse?> ChangeOrderCardItemAsync(int id, int userId, ChangeOrderCardItemRequest request)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems
                .Include(cd => cd.Column)
                .ThenInclude(bc => bc.Board)
                .FirstOrDefaultAsync(cd => cd.CardId == id);
            if (cardItem == null || cardItem.Column.Board.UserId != userId) 
                return null;

            // Validate the new order value to ensure it is within the valid range
            var cardInColumn = await context.CardItems.CountAsync(ci => ci.ColumnId == cardItem.ColumnId);
            if (request.Order < 0 || request.Order > cardInColumn - 1)
                return null;

            int oldOrder = cardItem.Order;
            int newOrder = request.Order;

            if(oldOrder == newOrder) return cardItem.ToDto();

            // Retrieve all card items in the same column that have an order between the old and new order values
            await ReorderAllCardsInColumn(cardItem, newOrder, oldOrder);

            // Update the order of the card item based on the request data
            cardItem.Order = request.Order;

            // Save changes to the database
            await context.SaveChangesAsync();
            return cardItem.ToDto();
        }

        /// <summary>
        /// Moves a card item to a different column in the database based on the provided ID and request data, and returns the updated card item as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<CardItemResponse?> MoveCardItemColumnAsync(int id, int userId, MoveCardItemColumnRequest request)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems
                .Include(cd => cd.Column)
                .ThenInclude(bc => bc.Board)
                .FirstOrDefaultAsync(cd => cd.CardId == id);
            if (cardItem == null || cardItem.Column.Board.UserId != userId)
                return null;

            // Find the destination column in the database by its ID
            var destinationColumn = await context.BoardColumns
            .Include(bc => bc.Board)
            .FirstOrDefaultAsync(bc => bc.ColumnId == request.DestinationColumnId);

            if (destinationColumn == null || destinationColumn.Board.UserId != userId)
                return null;
            if (destinationColumn.ColumnId == cardItem.ColumnId) return cardItem.ToDto();

            int newColumnId = request.DestinationColumnId;
            int oldColumnId = cardItem.ColumnId;
            int oldOrder = cardItem.Order;

            // Retrieve all card items in the old column that have an order greater than the moved card item's order
            var oldColumnCards = await context.CardItems
                .Where(ci => ci.ColumnId == oldColumnId &&
                ci.Order > oldOrder)
                .ToListAsync();

            foreach (var card in oldColumnCards)
                card.Order--;

            // Retrieve all card items in the destination column to determine the new order for the moved card item
            var cards = await context.CardItems
            .Where(c => c.ColumnId == newColumnId)
            .ToListAsync();

            // Update the column ID of the card item based on the request data
            cardItem.ColumnId = newColumnId;

            if (cards.Count == 0)
            {
                cardItem.Order = 0;
            }
            else
            {
                cardItem.Order = cards.Max(c => c.Order) + 1;
            }

            // Save changes to the database
            await context.SaveChangesAsync();
            return cardItem.ToDto();
        }

        /// <summary>
        /// Deletes a card item from the database by its ID and returns a boolean indicating whether the deletion was successful.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCardItemAsync(int id, int userId)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems
                .Include(cd => cd.Column)
                .ThenInclude(bc => bc.Board)
                .FirstOrDefaultAsync(cd => cd.CardId == id);
            if (cardItem == null || cardItem.Column.Board.UserId != userId)
                return false;

            await ReorderCardsAfterDelete(cardItem);

            // Remove the card item from the database context and save changes
            context.CardItems.Remove(cardItem);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Changes the order of all card items in the same column based on the provided card item, new order, and old order values. This method adjusts the order of other card items in the column to maintain a consistent ordering.
        /// </summary>
        /// <param name="cardItem"></param>
        /// <param name="newOrder"></param>
        /// <param name="oldOrder"></param>
        /// <returns></returns>
        private async Task ReorderAllCardsInColumn(CardItem cardItem, int newOrder, int oldOrder)
        {
            // If the new order is greater than the old order, decrement the order of card items that are between the old and new order values
            if (newOrder > oldOrder)
            {
                List<CardItem> cardsToUpper = await context.CardItems
                .Where(
                ci => ci.ColumnId == cardItem.ColumnId &&
                ci.Order <= newOrder &&
                ci.Order > oldOrder)
                .ToListAsync();

                foreach (var card in cardsToUpper)
                    card.Order--;
            }
            else
            {
                List<CardItem> cardsToLower = await context.CardItems
                .Where(
                ci => ci.ColumnId == cardItem.ColumnId &&
                ci.Order >= newOrder &&
                ci.Order < oldOrder)
                .ToListAsync();

                foreach (var card in cardsToLower)
                    card.Order++;
            }
        }

        /// <summary>
        /// Changes the order of all cards items in the same column based on the provided card item to be deleted. This method adjust the order of other card items in the column to maintaing a consistent ordering
        /// </summary>
        /// <param name="deletedCard"></param>
        /// <returns></returns>
        private async Task ReorderCardsAfterDelete(CardItem deletedCard)
        {
            // Get al cards that comes after the deleted card
            var cardsAfter = await context.CardItems.
                Where(ci => ci.ColumnId == deletedCard.ColumnId && ci.Order > deletedCard.Order).
                ToListAsync();

            // Reduce the order of the obtained cards to maintain consistency
            foreach(var card in cardsAfter)
                card.Order--;
        }
    }
}
