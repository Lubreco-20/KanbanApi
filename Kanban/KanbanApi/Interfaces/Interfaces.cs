using KanbanApi.DTOs;
using KanbanApi.Models;

namespace KanbanApi.Interfaces
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest request);
        Task<UserResponse?> UpdateUserAsync(int id, UpdateUserRequest request);
        Task<UserResponse?> UpdatePasswordAsync(int id, UpdatePasswordRequest request);
        Task<UserResponse?> GetUserByIdAsync(int id, int authenticateId);
        Task<bool> DeleteUserAsync(int id, int authenticateId);
    }

    public interface IBoardService
    {
        Task<BoardResponse?> CreateBoardAsync(int userId, CreateBoardRequest request);
        Task<BoardResponse?> UpdateBoardAsync(int id, int userId, UpdateBoardRequest request);
        Task<BoardResponse?> GetBoardByIdAsync(int id, int userId);
        Task<PageResponse<BoardResponse>> GetBoardsByUserIdAsync(int userId, int page, int pageSize);
        Task<bool> DeleteBoardAsync(int id, int userId);
    }

    public interface IBoardColumnService
    {
        Task<BoardColumnResponse?> CreateBoardColumnAsync(int boardId, int userId, CreateBoardColumnRequest request);
        Task<BoardColumnResponse?> UpdateBoardColumnAsync(int id, int userId, UpdateBoardColumnRequest request);
        Task<BoardColumnResponse?> GetBoardColumnByIdAsync(int id, int userId);
        Task<IEnumerable<BoardColumnResponse>> GetBoardColumnsByBoardIdAsync(int boardId, int userId);
        Task<BoardColumnResponse?> ChangeOrderBoardColumnAsync(int id, int userId, ChangeOrderBoardColumnRequest request);
        Task<bool> DeleteBoardColumnAsync(int id, int userId);
    }

    public interface ICardItemService
    {
        Task<CardItemResponse?> CreateCardItemAsync(int columnId, int userId, CreateCardItemRequest request);
        Task<CardItemResponse?> UpdateCardItemAsync(int id, int userId, UpdateCardItemRequest request);
        Task<CardItemResponse?> GetCardItemByIdAsync(int id, int userId);
        Task<PageResponse<CardItemResponse>> GetCardItemsByColumnIdAsync(int columnId, int userId, int page, int pageSize);
        Task<CardItemResponse?> ChangeOrderCardItemAsync(int id, int userId, ChangeOrderCardItemRequest request);
        Task<CardItemResponse?> MoveCardItemColumnAsync(int id, int userId, MoveCardItemColumnRequest request);
        Task<bool> DeleteCardItemAsync(int id, int userId);
    }

    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
    }

    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
        Task<bool> LogoutAsync(LogoutRequest request);
        Task<LoginResponse?> RefreshAsync(RefreshRequest request);
    }
}