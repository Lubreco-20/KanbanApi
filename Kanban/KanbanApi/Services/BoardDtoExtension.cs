using KanbanApi.DTOs;
using KanbanApi.Models;

namespace KanbanApi.Services
{
    public static class BoardDtoExtension
    {
        public static BoardResponse ToDto(this Board board)
        {
            return new BoardResponse
            (
                board.BoardId,
                board.BoardName,
                board.BoardDescription,
                board.Columns?.Select(column => column.ToDto()).ToList() ?? new List<BoardColumnResponse>()
            );
        }
    }
}
