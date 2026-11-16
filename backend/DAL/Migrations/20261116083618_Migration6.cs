using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Migration6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_AspNetUsers_UserId",
                table: "Borrowings");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_AspNetUsers_UserId",
                table: "Borrowings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_AspNetUsers_UserId",
                table: "Borrowings");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_AspNetUsers_UserId",
                table: "Borrowings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
