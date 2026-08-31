using System.ComponentModel.DataAnnotations;

namespace KanbanApi.DTOs
{
    public sealed record PageRequest(
        [MinLength(1, ErrorMessage = "The page limit must be at least 1.")]
        int Page = 1,

        [Range(1, 100, ErrorMessage = "The page size must be between 1 and 100.")]
        int PageSize = 10
        );

    public sealed record PageResponse<T>(
        List<T> items,
        int Page,
        int PageSize,
        int TotalCount,
        int TotalPages
        );
}
