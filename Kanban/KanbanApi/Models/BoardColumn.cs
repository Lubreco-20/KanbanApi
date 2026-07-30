namespace KanbanApi.Models
{
    public class BoardColumn
    {
        public int ColumnId { get; set; }
        public int Order { get; set; }

        // Foreign key to the Board
        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;

        // Cards in the column
        public ICollection<CardItem> CardItems { get; set; } = new List<CardItem>();
    }
}
