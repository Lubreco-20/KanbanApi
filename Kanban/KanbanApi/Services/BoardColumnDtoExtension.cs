using KanbanApi.DTOs;
using KanbanApi.Models;

namespace KanbanApi.Services
{
    public static class BoardColumnDtoExtension
    {
        public static BoardColumnResponse ToDto(this BoardColumn boardColumn)
        {
            return new BoardColumnResponse(
                boardColumn.ColumnId,
                boardColumn.ColumnName,
                boardColumn.Order,
                boardColumn.CardItems?.Select(ci => ci.ToDto()).ToList() ?? new List<CardItemResponse>() // Convert CardItems to DTOs
            );
        }
    }
}
