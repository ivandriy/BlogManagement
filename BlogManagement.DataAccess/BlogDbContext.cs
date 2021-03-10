using System;
using System.Linq;
using BlogManagement.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.DataAccess
{
    public class BlogDbContext : IdentityDbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options): base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.CategoryPosts)
                .UsingEntity(x => x.ToTable("PostCategoriesMapping"));
        
            #region DataSeed
        
            var categories = new[]
            {
                new Category {CategoryId = 1, Name = ".NET Core"},
                new Category {CategoryId = 2, Name = "Azure"},
                new Category {CategoryId = 3, Name = "Microservices"}
            };
            builder.Entity<Category>().HasData(categories);
        
            var themes = new[]
            {
                new Theme {ThemeId = 1, ThemeName = "Standard"},
                new Theme {ThemeId = 2, ThemeName = "Dark"}
            };
            builder.Entity<Theme>().HasData(themes);

            var blogs = new[]
            {
                new Blog {BlogId = 1, Name = "Andrii's Blog", ThemeId = themes.Last().ThemeId},
                new Blog {BlogId = 2, Name = "Ardalis", ThemeId = themes.First().ThemeId}

            };
            builder.Entity<Blog>().HasData(blogs);


            var authors = new[]
            {
                new Author
                {
                    AuthorId = 1, Email = "andrii@ivanskiy.com", FirstName = "Andrii", LastName = "Ivanskiy", BlogId = 1
                },
                new Author
                {
                    AuthorId = 2, Email = "steve_smith@ardalis.com", FirstName = "Steve", LastName = "Smith", BlogId = 2
                }
            };
            builder.Entity<Author>().HasData(authors);
        
            var posts = new[]
            {
                new Post
                {
                    BlogId = 1,
                    PostId = 1,
                    Title = "What's new in .NET 5",
                    Content = "Some blog about new features in .NET 5",
                    CreatedOn = DateTimeOffset.Parse("2020-12-01"),
                    UpdatedOn = DateTimeOffset.Parse("2020-12-01"),
                    UserName = authors.First().FirstName + ' ' + authors.First().LastName
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 2,
                    Title = "Azure ServiceBus - how to use it",
                    Content = "Azure ServiceBus how-to for .NET Core developers",
                    CreatedOn = DateTimeOffset.Parse("2020-12-10"),
                    UpdatedOn = DateTimeOffset.Parse("2020-12-11"),
                    UserName = authors.First().FirstName + ' ' + authors.First().LastName
                },
                new Post
                {
                    BlogId = 1,
                    PostId = 3,
                    Title = ".NET Core Microservices in Azure",
                    Content = "How to develop your first .NET Core microservice and deploy it to Azure",
                    CreatedOn = DateTimeOffset.Parse("2021-02-01"),
                    UpdatedOn = DateTimeOffset.Parse("2020-02-02"),
                    UserName = authors.First().FirstName + ' ' + authors.First().LastName
                },
                new Post
                {
                    BlogId = 2,
                    PostId = 4,
                    Title = "Design patterns in .NET",
                    Content = "Design patterns overview for .NET developers",
                    CreatedOn = DateTimeOffset.Parse("2018-04-01"),
                    UpdatedOn = DateTimeOffset.Parse("2019-07-05"),
                    UserName = authors.Last().FirstName + ' ' + authors.Last().LastName
                },
                new Post
                {
                    BlogId = 2,
                    PostId = 5,
                    Title = "DDD and Microservices",
                    Content = "How to use Domain-Driven Design to build microservices",
                    CreatedOn = DateTimeOffset.Parse("2019-03-16"),
                    UpdatedOn = DateTimeOffset.Parse("2020-04-01"),
                    UserName = authors.Last().FirstName + ' ' + authors.Last().LastName
                },
                new Post
                {
                    BlogId = 2,
                    PostId = 6,
                    Title = ".NET Core Best Practices",
                    Content = "Some best practices how to develop your .NET Core applications",
                    CreatedOn = DateTimeOffset.Parse("2021-03-01"),
                    UpdatedOn = DateTimeOffset.Parse("2021-03-01"),
                    UserName = authors.Last().FirstName + ' ' + authors.Last().LastName
                }
            };
            builder.Entity<Post>().HasData(posts);
            
            builder
                .Entity<Post>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.CategoryPosts)
                .UsingEntity(j => j.HasData(
                    new { CategoriesCategoryId = 1, CategoryPostsPostId = 1 },
                    new { CategoriesCategoryId = 1, CategoryPostsPostId = 2 },
                    new { CategoriesCategoryId = 2, CategoryPostsPostId = 2 },
                    new { CategoriesCategoryId = 1, CategoryPostsPostId = 3 },
                    new { CategoriesCategoryId = 2, CategoryPostsPostId = 3 },
                    new { CategoriesCategoryId = 3, CategoryPostsPostId = 3 },
                    new { CategoriesCategoryId = 1, CategoryPostsPostId = 4 },
                    new { CategoriesCategoryId = 1, CategoryPostsPostId = 5 },
                    new { CategoriesCategoryId = 3, CategoryPostsPostId = 5 },
                    new { CategoriesCategoryId = 1, CategoryPostsPostId = 6 }
                    
                ));
            
            #endregion
            
            base.OnModelCreating(builder);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        
        public DbSet<Theme> Themes { get; set; }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<Author> Authors { get; set; }
    }
}