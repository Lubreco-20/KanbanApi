namespace KanbanApi.Models
{
    public class CardItem
    {
        public int CardId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }

        // Foreign key to the BoardColumn
        public int ColumnId { get; set; }
        public BoardColumn Column { get; set; } = null!;
    }
}
