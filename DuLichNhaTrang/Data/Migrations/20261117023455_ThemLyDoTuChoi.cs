using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DuLichNhaTrang.Data.Migrations
{
    /// <inheritdoc />
    public partial class ThemLyDoTuChoi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDoTuChoi",
                table: "Posts",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LyDoTuChoi",
                table: "Posts");
        }
    }
}
