using System.ComponentModel.DataAnnotations;

namespace KanbanApi.Models
{
    public class Board
    {
        [Key] public int BoardId { get; set; }
        public string BoardName { get; set; } = string.Empty;
        public string BoardDescription { get; set; } = string.Empty;

        // Foreign key to the User
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        // Columns in the board
        public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();
    }
}
