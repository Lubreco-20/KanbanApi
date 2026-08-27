using System.ComponentModel.DataAnnotations;

namespace KanbanApi.DTOs
{
    public sealed record CreateBoardRequest(
        [Required(ErrorMessage = "A board name is required")]
        [MaxLength(255)]
        string BoardName,

        [MaxLength(500)]
        string BoardDescription
        );

    public sealed record UpdateBoardRequest(
        [Required(ErrorMessage = "A board name is required")]
        [MaxLength(255)]
        string BoardName,

        [MaxLength(500)]
        string BoardDescription
        );

    public sealed record BoardResponse(
        int Id,
        string BoardName,
        string BoardDescription,
        List<BoardColumnResponse> BoardColumns
        );
}
