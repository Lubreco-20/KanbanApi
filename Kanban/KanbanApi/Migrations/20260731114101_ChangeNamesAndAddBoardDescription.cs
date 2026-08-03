using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KanbanApi.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNamesAndAddBoardDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "BoardColumns",
                newName: "ColumnName");

            migrationBuilder.AddColumn<string>(
                name: "BoardDescription",
                table: "Boards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoardDescription",
                table: "Boards");

            migrationBuilder.RenameColumn(
                name: "ColumnName",
                table: "BoardColumns",
                newName: "Name");
        }
    }
}
