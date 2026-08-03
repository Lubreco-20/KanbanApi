using System.ComponentModel.DataAnnotations;

namespace KanbanApi.Models
{
    public class BoardColumn
    {
        [Key] public int ColumnId { get; set; }
        public string ColumnName { get; set; } = string.Empty;
        public int Order { get; set; }

        // Foreign key to the Board
        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;

        // Cards in the column
        public ICollection<CardItem> CardItems { get; set; } = new List<CardItem>();
    }
}
