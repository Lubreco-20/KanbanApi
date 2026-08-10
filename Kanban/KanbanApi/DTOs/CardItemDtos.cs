namespace KanbanApi.DTOs
{
    public sealed record CreateCardItemRequest(
        string Title,
        string Description
        );

    public sealed record UpdateCardItemRequest(
        string Title,
        string Description
        );

    public sealed record ChangeOrderCardItemRequest(
        int Order
        );

    public sealed record MoveCardItemColumnRequest(
        int DestinationColumnId
        );

    public sealed record CardItemResponse(
        int Id,
        string Title,
        string Description,
        int Order
        );
}
