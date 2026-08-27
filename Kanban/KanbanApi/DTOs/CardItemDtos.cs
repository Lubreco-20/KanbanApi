using System.ComponentModel.DataAnnotations;

namespace KanbanApi.DTOs
{
    public sealed record CreateCardItemRequest(
        [Required(ErrorMessage = "A card title is required")]
        [MaxLength(100)]
        string Title,

        [MaxLength(500)]
        string Description
        );

    public sealed record UpdateCardItemRequest(
        [Required(ErrorMessage = "A card title is required")]
        [MaxLength(100)]
        string Title,

        [MaxLength(500)]
        string Description
        );

    public sealed record ChangeOrderCardItemRequest(
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please, enter a valid number")]
        int Order
        );

    public sealed record MoveCardItemColumnRequest(
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please, enter a valid column id number")]
        int DestinationColumnId
        );

    public sealed record CardItemResponse(
        int Id,
        string Title,
        string Description,
        int Order
        );
}
