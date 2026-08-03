using KanbanApi.Models;
using KanbanApi.DTOs;
using KanbanApi.Data;

namespace KanbanApi.Services
{
    public class CardItemService
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
        public async Task<CardItemResponse> CareateCardItemAsync(int columnId, CreateCardItemRequest request)
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
        public async Task<CardItemResponse> UpdateCardItemAsync(int id, UpdateCardItemRequest request)
        {
            // Find the card item in the database by its ID
            var cardItem = await context.CardItems.FindAsync(id);
            if (cardItem == null)
                throw new InvalidOperationException("Card item not found");

            // Update the card item properties based on the request data
            cardItem.Title = request.Title;
            cardItem.Description = request.Description;
            cardItem.Order = request.Order;

            // Save changes to the database
            await context.SaveChangesAsync();
            return cardItem.ToDto();
        }
    }
}
