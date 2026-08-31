using KanbanApi.DTOs;
using KanbanApi.Data;
using KanbanApi.Models;
using KanbanApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanbanApi.Services
{
    public class BoardService : IBoardService
    {
        private readonly KanbanDbContext context;

        /// <summary>
        /// Initializes a new instance of the BoardService class with the specified database context.
        /// </summary>
        /// <param name="context"></param>
        public BoardService(KanbanDbContext context) => this.context = context;

        /// <summary>
        /// Creates a new board based on the provided request and saves it to the database.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<BoardResponse?> CreateBoardAsync(int userId, CreateBoardRequest request)
        {
            // Check if the user exists
            var user = await context.Users.FindAsync(userId);
            if (user == null) return null;

            // Create a new Board entity using the data from the request
            var board = new Board
            {
                BoardName = request.BoardName,
                BoardDescription = request.BoardDescription,
                UserId = userId
            };

            // Add the new board to the database context and save changes
            context.Boards.Add(board);
            await context.SaveChangesAsync();

            // Convert the saved board entity to a BoardResponse DTO and return it
            return board.ToDto();
        }

        /// <summary>
        /// Updates an existing board identified by its ID with the data provided in the request.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BoardResponse?> UpdateBoardAsync(int id, int userId, UpdateBoardRequest request)
        {
            // Find the existing board by its ID
            var board = await context.Boards.FindAsync(id);
            if (board == null || board.UserId != userId)
                return null;

            // Update the board's properties with the data from the request
            board.BoardName = request.BoardName;
            board.BoardDescription = request.BoardDescription;

            // Save changes to the database
            await context.SaveChangesAsync();

            // Convert the updated board entity to a BoardResponse DTO and return it
            return board.ToDto();
        }

        /// <summary>
        /// Get the board by the Id from the Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<BoardResponse?> GetBoardByIdAsync(int id, int userId)
        {
            // Find the board by its ID
            var board = await context.Boards.FindAsync(id);
            if (board == null || board.UserId != userId)
                return null;

            // Convert the board entity to a BoardResponse DTO and return it
            return board.ToDto();
        }

        /// <summary>
        /// Retrieves all boards associated with a specific user ID and returns them as a collection of BoardResponse DTOs.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<PageResponse<BoardResponse>> GetBoardsByUserIdAsync(int userId, int page, int pageSize)
        {
            // Retrieve all boards associated with the specified user ID and count the amount
            var query = context.Boards.Where(b => b.UserId == userId);
            var totalCount = await query.CountAsync();

            // We make a query to get all the items in the page
            var boards = await query
                .OrderBy(b => b.BoardId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Convert the list of board entities to a list of BoardResponse DTOs and return it
            return new PageResponse<BoardResponse>(
                boards.Select(b => b.ToDto()).ToList(),
                page,
                pageSize,
                totalCount,
                totalPages);
        }

        /// <summary>
        /// Deletes an existing board identified by its ID from the database.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteBoardAsync(int id, int userId)
        {
            // Find the board by its ID
            var board = await context.Boards.FindAsync(id);
            if (board == null || board.UserId != userId)
                return false;

            // Remove the board from the database context and save changes
            context.Boards.Remove(board);
            await context.SaveChangesAsync();

            // Return true to indicate successful deletion
            return true;
        }
    }
}
