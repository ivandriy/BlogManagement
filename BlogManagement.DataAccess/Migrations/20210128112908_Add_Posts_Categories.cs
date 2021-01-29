using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BlogManagement.DataAccess.Migrations
{
    public partial class Add_Posts_Categories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "PostCategoriesMapping",
                columns: table => new
                {
                    CategoriesCategoryId = table.Column<int>(type: "integer", nullable: false),
                    CategoryPostsPostId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategoriesMapping", x => new { x.CategoriesCategoryId, x.CategoryPostsPostId });
                    table.ForeignKey(
                        name: "FK_PostCategoriesMapping_Categories_CategoriesCategoryId",
                        column: x => x.CategoriesCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCategoriesMapping_Posts_CategoryPostsPostId",
                        column: x => x.CategoryPostsPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostCategoriesMapping_CategoryPostsPostId",
                table: "PostCategoriesMapping",
                column: "CategoryPostsPostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostCategoriesMapping");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
