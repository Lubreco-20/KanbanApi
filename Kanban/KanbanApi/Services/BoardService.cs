using KanbanApi.DTOs;
using KanbanApi.Data;
using KanbanApi.Models;

namespace KanbanApi.Services
{
    public class BoardService
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
        public async Task<BoardResponse> CreateBoardAsync(int userId,CreateBoardRequest request)
        {
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
        public async Task<BoardResponse> UpdateBoardAsync(int id, UpdateBoardRequest request)
        {
            // Find the existing board by its ID
            var board = await context.Boards.FindAsync(id);
            if (board == null)
                throw new InvalidOperationException("Board not found");

            // Update the board's properties with the data from the request
            board.BoardName = request.BoardName;
            board.BoardDescription = request.BoardDescription;

            // Save changes to the database
            await context.SaveChangesAsync();

            // Convert the updated board entity to a BoardResponse DTO and return it
            return board.ToDto();
        }
    }
}
