using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogManagement.Migrations
{
    public partial class PostsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Topic",
                table: "Posts",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Posts",
                newName: "Content");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Posts",
                newName: "Topic");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Posts",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Posts",
                type: "text",
                nullable: true);
        }
    }
}
