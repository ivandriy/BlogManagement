using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogManagement.DataAccess.Migrations
{
    public partial class SetThemeNameUniq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Themes_ThemeName",
                table: "Themes",
                column: "ThemeName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Themes_ThemeName",
                table: "Themes");
        }
    }
}
