using System.ComponentModel.DataAnnotations;

namespace KanbanApi.DTOs
{
    public sealed record CreateBoardColumnRequest(
        [Required(ErrorMessage = "A column name is required")]
        [MaxLength(100)]
        string ColumnName
        );

    public sealed record UpdateBoardColumnRequest(
        [Required(ErrorMessage = "A column name is required")]
        [MaxLength(100)]
        string ColumnName
        );

    public sealed record ChangeOrderBoardColumnRequest(
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please, enter a valid number")]
        int Order
        );

    public sealed record BoardColumnResponse(
        int Id,
        string ColumnName,
        int Order,
        List<CardItemResponse> CardItems
        );
}
