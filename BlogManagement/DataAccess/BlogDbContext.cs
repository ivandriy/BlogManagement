using BlogManagement.Models;
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
            
            base.OnModelCreating(builder);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        
        public DbSet<Theme> Themes { get; set; }
        
        public DbSet<Category> Categories { get; set; }

        public DbSet<Author> Authors { get; set; }
    }
}