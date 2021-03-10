using Microsoft.EntityFrameworkCore.Migrations;

namespace BlogManagement.DataAccess.Migrations
{
    public partial class Add_DataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Themes_ThemeId",
                table: "Blogs");

            migrationBuilder.AlterColumn<int>(
                name: "ThemeId",
                table: "Blogs",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, ".NET Core" },
                    { 2, "Azure" },
                    { 3, "Microservices" }
                });

            migrationBuilder.InsertData(
                table: "Themes",
                columns: new[] { "ThemeId", "ThemeName" },
                values: new object[,]
                {
                    { 1, "Standard" },
                    { 2, "Dark" }
                });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "BlogId", "Name", "ThemeId" },
                values: new object[,]
                {
                    { 2, "Ardalis", 1 },
                    { 1, "Andrii's Blog", 2 }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "AuthorId", "BlogId", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 2, 2, "steve_smith@ardalis.com", "Steve", "Smith" },
                    { 1, 1, "andrii@ivanskiy.com", "Andrii", "Ivanskiy" }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "BlogId", "Content", "CreatedOn", "Title", "UpdatedOn", "UserName" },
                values: new object[,]
                {
                    { 4, 2, "Design patterns overview for .NET developers", null, "Design patterns in .NET", null, "Steve Smith" },
                    { 5, 2, "How to use Domain-Driven Design to build microservices", null, "DDD and Microservices", null, "Steve Smith" },
                    { 6, 2, "Some best practices how to develop your .NET Core applications", null, ".NET Core Best Practices", null, "Steve Smith" },
                    { 1, 1, "Some blog about new features in .NET 5", null, "What's new in .NET 5", null, "Andrii Ivanskiy" },
                    { 2, 1, "Azure ServiceBus how-to for .NET Core developers", null, "Azure ServiceBus - how to use it", null, "Andrii Ivanskiy" },
                    { 3, 1, "How to develop your first .NET Core microservice and deploy it to Azure", null, ".NET Core Microservices in Azure", null, "Andrii Ivanskiy" }
                });

            migrationBuilder.InsertData(
                table: "PostCategoriesMapping",
                columns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 1, 5 },
                    { 3, 5 },
                    { 1, 6 },
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 2 },
                    { 1, 3 },
                    { 2, 3 },
                    { 3, 3 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Themes_ThemeId",
                table: "Blogs",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Themes_ThemeId",
                table: "Blogs");

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "AuthorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 1, 5 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 1, 6 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "PostCategoriesMapping",
                keyColumns: new[] { "CategoriesCategoryId", "CategoryPostsPostId" },
                keyValues: new object[] { 3, 5 });

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "BlogId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "BlogId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Themes",
                keyColumn: "ThemeId",
                keyValue: 2);

            migrationBuilder.AlterColumn<int>(
                name: "ThemeId",
                table: "Blogs",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Themes_ThemeId",
                table: "Blogs",
                column: "ThemeId",
                principalTable: "Themes",
                principalColumn: "ThemeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
