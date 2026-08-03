using KanbanApi.DTOs;
using KanbanApi.Data;
using KanbanApi.Models;

namespace KanbanApi.Services
{
    public class BoardColumnService
    {
        private readonly KanbanDbContext context;

        /// <summary>
        /// Initializes a new instance of the BoardColumnService class with the specified database context.
        /// </summary>
        /// <param name="context"></param>
        public BoardColumnService(KanbanDbContext context) => this.context = context;

        /// <summary>
        /// Creates a new board column in the database based on the provided request and returns the created board column as a response DTO.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BoardColumnResponse> CreateBoardColumnAsync(int boardId, CreateBoardColumnRequest request)
        {
            var boardColumn = new BoardColumn
            {
                ColumnName = request.ColumnName,
                Order = request.Order,
                BoardId = boardId
            };

            context.BoardColumns.Add(boardColumn);
            await context.SaveChangesAsync();

            return boardColumn.ToDto();
        }

        /// <summary>
        /// Updates an existing board column in the database based on the provided ID and request, and returns the updated board column as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BoardColumnResponse> UpdateBoardColumnAsync(int id, UpdateBoardColumnRequest request)
        {
            var boardColumn = await context.BoardColumns.FindAsync(id);
            if (boardColumn == null)
                throw new InvalidOperationException("Board column not found");

            boardColumn.ColumnName = request.ColumnName;
            boardColumn.Order = request.Order;

            await context.SaveChangesAsync();
            return boardColumn.ToDto();
        }
    }
}
