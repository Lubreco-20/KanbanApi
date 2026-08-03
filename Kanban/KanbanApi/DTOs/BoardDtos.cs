namespace KanbanApi.DTOs
{
    public sealed record CreateBoardRequest(
        string BoardName,
        string BoardDescription
        );

    public sealed record UpdateBoardRequest(
        string BoardName,
        string BoardDescription
        );

    public sealed record BoardResponse(
        int Id,
        string BoardName,
        string BoardDescription,
        List<BoardColumnResponse> BoardColumns
        );
}
