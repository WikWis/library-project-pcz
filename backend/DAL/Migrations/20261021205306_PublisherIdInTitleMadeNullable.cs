using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class PublisherIdInTitleMadeNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Publishers_PublisherId",
                table: "Titles");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                table: "Titles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Publishers_PublisherId",
                table: "Titles",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Publishers_PublisherId",
                table: "Titles");

            migrationBuilder.AlterColumn<int>(
                name: "PublisherId",
                table: "Titles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Publishers_PublisherId",
                table: "Titles",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
