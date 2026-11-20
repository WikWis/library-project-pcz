using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Migration7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pieces");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Borrowings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Borrowings");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Pieces",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
