using Microsoft.EntityFrameworkCore;
using KanbanApi.DTOs;
using KanbanApi.Data;
using KanbanApi.Models;
using KanbanApi.Interfaces;

namespace KanbanApi.Services
{
    public class BoardColumnService : IBoardColumnService
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
        public async Task<BoardColumnResponse?> CreateBoardColumnAsync(int boardId, int userId, CreateBoardColumnRequest request)
        {
            // Check if the board exists
            var board = await context.Boards.FindAsync(boardId);
            if (board == null || board.UserId != userId) 
                return null;

            var columns = await context.BoardColumns.
                Where(b => b.BoardId == board.BoardId).
                ToListAsync();

            int newOrder = columns.Any() ? columns.Max(c => c.Order) + 1 : 0;

            // Create a new BoardColumn entity based on the request and the provided board ID
            var boardColumn = new BoardColumn
            {
                ColumnName = request.ColumnName,
                Order = newOrder,
                BoardId = boardId
            };

            // Add the new board column to the database context and save changes
            context.BoardColumns.Add(boardColumn);
            await context.SaveChangesAsync();

            // Return the created board column as a response DTO
            return boardColumn.ToDto();
        }

        /// <summary>
        /// Updates an existing board column in the database based on the provided ID and request, and returns the updated board column as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BoardColumnResponse?> UpdateBoardColumnAsync(int id, int userId, UpdateBoardColumnRequest request)
        {
            // Check if the board column exists
            var boardColumn = await context.BoardColumns
                .Include(bc => bc.Board)
                .Where(bc => bc.ColumnId == id)
                .FirstOrDefaultAsync();
            if (boardColumn == null || boardColumn.Board.UserId != userId)
                return null;

            // Update the board column properties based on the request
            boardColumn.ColumnName = request.ColumnName;

            // Save the changes to the database
            await context.SaveChangesAsync();
            return boardColumn.ToDto();
        }

        /// <summary>
        /// Retrieves a board column from the database based on the provided ID and returns it as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BoardColumnResponse?> GetBoardColumnByIdAsync(int id, int userId)
        {
            // Check if the board column exists
            var boardColumn = await context.BoardColumns
                .Include(bc => bc.Board)
                .Where(bc => bc.ColumnId == id)
                .FirstOrDefaultAsync();
            if (boardColumn == null || boardColumn.Board.UserId != userId)
                return null;

            // Return the board column as a response DTO
            return boardColumn.ToDto();
        }

        /// <summary>
        /// Retrieves all board columns associated with a specific board ID from the database and returns them as a collection of response DTOs.
        /// </summary>
        /// <param name="boardId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<BoardColumnResponse>> GetBoardColumnsByBoardIdAsync(int boardId, int userId)
        {
            // Retrieve all board columns associated with the specified board ID
            var boardColumns = await context.BoardColumns
                .Where(bc => bc.BoardId == boardId && bc.Board.UserId == userId)
                .ToListAsync<BoardColumn>();

            // Convert the board columns to response DTOs and return them
            return boardColumns.Select(bc => bc.ToDto());
        }

        /// <summary>
        /// Moves a board column to a new order based on the provided ID and request, and returns the updated board column as a response DTO.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BoardColumnResponse?> ChangeOrderBoardColumnAsync(int id, int userId, ChangeOrderBoardColumnRequest request)
        {
            // Check if the board column exists
            var column = await context.BoardColumns
                .Include(bc => bc.Board)
                .Where(bc => bc.ColumnId == id)
                .FirstOrDefaultAsync();
            if (column == null || column.Board.UserId != userId)
                return null;

            // Validate the new order value to ensure it is within the valid range
            var amountColumns = await context.BoardColumns.CountAsync(bc => bc.BoardId == column.BoardId);
            if (request.Order < 0 || request.Order > amountColumns - 1)
                return null;

            int newOrder = request.Order;
            int oldOrder = column.Order;

            if (oldOrder == newOrder) return column.ToDto();

            await ReorderBoardColumnAsync(column, newOrder, oldOrder);

            // Update the board column's order based on the request
            column.Order = newOrder;

            // Save the changes to the database
            await context.SaveChangesAsync();
            return column.ToDto();
        }

        /// <summary>
        /// Deletes a board column from the database based on the provided ID and returns a boolean indicating whether the deletion was successful.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBoardColumnAsync(int id, int userId)
        {
            // Check if the board column exists
            var boardColumn = await context.BoardColumns
                .Include(bc => bc.Board)
                .Where(bc => bc.ColumnId == id)
                .FirstOrDefaultAsync();
            if (boardColumn == null || boardColumn.Board.UserId != userId)
                return false;

            await ReorderColumnsAfterDelete(boardColumn);

            // Remove the board column from the database
            context.BoardColumns.Remove(boardColumn);
            await context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Reorders the board columns in the database based on the provided column, new order, and old order. This method adjusts the order of other columns to maintain a consistent ordering.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="newOrder"></param>
        /// <param name="oldOrder"></param>
        /// <returns></returns>
        private async Task ReorderBoardColumnAsync(BoardColumn column, int newOrder, int oldOrder)
        {
            if (newOrder > oldOrder)
            {
                List<BoardColumn> columnsToLeft = await context.BoardColumns
                .Where(
                bc => bc.BoardId == column.BoardId &&
                bc.Order <= newOrder &&
                bc.Order > oldOrder)
                .ToListAsync();

                foreach (var boardColumn in columnsToLeft)
                    boardColumn.Order--;
            }
            else
            {
                List<BoardColumn> columnsToRight = await context.BoardColumns
                .Where(
                bc => bc.BoardId == column.BoardId &&
                bc.Order >= newOrder &&
                bc.Order < oldOrder)
                .ToListAsync();

                foreach (var boardColumn in columnsToRight)
                    boardColumn.Order++;
            }
        }

        /// <summary>
        /// Changes the order of all board columns in the same board based on the provided column to be deleted. This method adjust the order of other columns in the board to maintaing a consistent ordering
        /// </summary>
        /// <param name="deletedColumn"></param>
        /// <returns></returns>
        private async Task ReorderColumnsAfterDelete(BoardColumn deletedColumn)
        {
            // Gets all columns with grater order value than the deleted column
            var columns = await context.BoardColumns.
                Where(bc => bc.BoardId == deletedColumn.BoardId && bc.Order > deletedColumn.Order).
                ToListAsync();

            // reduce the order of the provided columns
            foreach (var column in columns)
                column.Order--;
        }
    }
}