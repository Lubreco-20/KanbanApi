namespace KanbanApi.DTOs
{
    public sealed record CreateBoardColumnRequest(
        string ColumnName
        );

    public sealed record UpdateBoardColumnRequest(
        string ColumnName
        );

    public sealed record ChangeOrderBoardColumnRequest(
        int Order
        );

    public sealed record BoardColumnResponse(
        int Id,
        string ColumnName,
        int Order,
        List<CardItemResponse> CardItems
        );
}
