namespace KanbanApi.DTOs
{
    public sealed record CreateCardItemRequest(
        string Title,
        string Description,
        int Order
        );

    public sealed record UpdateCardItemRequest(
        string Title,
        string Description,
        int Order
        );

    public sealed record CardItemResponse(
        int Id,
        string Title,
        string Description,
        int Order
        );
}
