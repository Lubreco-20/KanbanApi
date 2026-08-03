using KanbanApi.Data;
using KanbanApi.DTOs;
using KanbanApi.Interfaces;
using KanbanApi.Models;
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
        public async Task<CardItemResponse> CreateCardItemAsync(int columnId, CreateCardItemRequest request)
        {
            // Create a new CardItem entity based on the request data
            var cardItem = new CardItem
            {
                Title = request.Title,
                Description = request.Description,
                Order = request.Order,
                ColumnId = columnId
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
        public async Task<CardItemResponse?> UpdateCardItemAsync(int id, UpdateCardItemRequest request)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems.FindAsync(id);
            if (cardItem == null)
                return null;

            // Update the card item properties based on the request data
            cardItem.Title = request.Title;
            cardItem.Description = request.Description;
            cardItem.Order = request.Order;

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
        public async Task<CardItemResponse?> GetCardItemByIdAsync(int id)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems.FindAsync(id);
            if (cardItem == null)
                return null;

            // Return the found card item as a response DTO
            return cardItem.ToDto();
        }

        /// <summary>
        /// Retrieves all card items associated with a specific column ID from the database and returns them as a collection of response DTOs.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CardItemResponse>> GetCardItemsByColumnIdAsync(int columnId)
        {
            // Retrieve all card items associated with the specified column ID
            var cardItems = await context.CardItems
                .Where(ci => ci.ColumnId == columnId)
                .ToListAsync();

            // Convert the list of card items to a list of response DTOs and return it
            return cardItems.Select(ci => ci.ToDto());
        }

        /// <summary>
        /// Deletes a card item from the database by its ID and returns a boolean indicating whether the deletion was successful.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteCardItemAsync(int id)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems.FindAsync(id);
            if (cardItem == null)
                return false;

            // Remove the card item from the database context and save changes
            context.CardItems.Remove(cardItem);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
