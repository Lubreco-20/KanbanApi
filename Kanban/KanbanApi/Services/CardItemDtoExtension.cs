using KanbanApi.DTOs;
using KanbanApi.Models;

namespace KanbanApi.Services
{
    public static class CardItemDtoExtension
    {
        /// <summary>
        /// Converts a CardItem entity to a CardItemResponse DTO.
        /// </summary>
        /// <param name="cardItem"></param>
        /// <returns></returns>
        public static CardItemResponse ToDto(this CardItem cardItem)
        {
            return new CardItemResponse(
                cardItem.CardId,
                cardItem.Title,
                cardItem.Description,
                cardItem.Order
            );
        }
    }
}
