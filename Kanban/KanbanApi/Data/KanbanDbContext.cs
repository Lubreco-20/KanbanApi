using Microsoft.EntityFrameworkCore;
using KanbanApi.Models;

namespace KanbanApi.Data
{
    public class KanbanDbContext : DbContext
    {
        // Constructor to initialize the DbContext with options
        public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options)
        {
        }

        // Define DbSet properties for each entity
        public DbSet<Board> Boards { get; set; }
        public DbSet<BoardColumn> BoardColumns { get; set; }
        public DbSet<CardItem> CardItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Username and Email should be unique
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // One User has many Boards, and a Board belongs to one User
            modelBuilder.Entity<Board>()
                .HasOne(u => u.User)
                .WithMany(b => b.Boards)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One Board has many BoardColumns, and a BoardColumn belongs to one Board
            modelBuilder.Entity<BoardColumn>()
                .HasOne(bc => bc.Board)
                .WithMany(b => b.Columns)
                .HasForeignKey(bc => bc.BoardId)
                .OnDelete(DeleteBehavior.Cascade);

            // One BoardColumn has many CardItems, and a CardItem belongs to one BoardColumn
            modelBuilder.Entity<CardItem>()
                .HasOne(ci => ci.Column)
                .WithMany(c => c.CardItems)
                .HasForeignKey(ci => ci.ColumnId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
