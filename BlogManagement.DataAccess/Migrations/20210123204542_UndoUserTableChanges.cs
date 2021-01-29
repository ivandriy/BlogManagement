using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogManagement.DataAccess.Migrations
{
    public partial class UndoUserTableChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Blogs_BlogId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BlogId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BlogId",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlogId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BlogId",
                table: "Users",
                column: "BlogId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Blogs_BlogId",
                table: "Users",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "BlogId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
