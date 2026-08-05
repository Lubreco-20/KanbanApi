using KanbanApi.DTOs;

namespace KanbanApi.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<UserResponse?> UpdatePasswordAsync(int id, UpdatePasswordRequest request);
        Task<UserResponse?> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
    }

    public interface IBoardService
    {
        Task<BoardResponse> CreateBoardAsync(int userId, CreateBoardRequest request);
        Task<BoardResponse?> UpdateBoardAsync(int id, UpdateBoardRequest request);
        Task<BoardResponse?> GetBoardByIdAsync(int id);
        Task<IEnumerable<BoardResponse>> GetBoardsByUserIdAsync(int userId);
        Task<bool> DeleteBoardAsync(int id);
    }

    public interface IBoardColumnService
    {
        Task<BoardColumnResponse> CreateBoardColumnAsync(int boardId, CreateBoardColumnRequest request);
        Task<BoardColumnResponse?> UpdateBoardColumnAsync(int id, UpdateBoardColumnRequest request);
        Task<BoardColumnResponse?> GetBoardColumnByIdAsync(int id);
        Task<IEnumerable<BoardColumnResponse>> GetBoardColumnsByBoardIdAsync(int boardId);
        Task<BoardColumnResponse?> ChangeOrderBoardColumnAsync(int id, ChangeOrderBoardColumnRequest request);
        Task<bool> DeleteBoardColumnAsync(int id);
    }

    public interface ICardItemService
    {
        Task<CardItemResponse> CreateCardItemAsync(int columnId, CreateCardItemRequest request);
        Task<CardItemResponse?> UpdateCardItemAsync(int id, UpdateCardItemRequest request);
        Task<CardItemResponse?> GetCardItemByIdAsync(int id);
        Task<IEnumerable<CardItemResponse>> GetCardItemsByColumnIdAsync(int columnId);
        Task<CardItemResponse?> ChangeOrderCardItemAsync(int id, ChangeOrderCardItemRequest request);
        Task<CardItemResponse?> MoveCardItemColumnAsync(int id, MoveCardItemColumnRequest request);
        Task<bool> DeleteCardItemAsync(int id);
    }
}