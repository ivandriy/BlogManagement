using BlogManagement.Abstract;
using BlogManagement.DataAccess;
using BlogManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Services
{
    public class BlogService : IBlogService
    {
        private readonly BlogDbContext _dbContext;

        public BlogService(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Blog
        public async Task<Blog> AddBlog(string name)
        {
            var newBlog = new Blog { Name = name };
            await _dbContext.Blogs.AddAsync(newBlog);
            await _dbContext.SaveChangesAsync();
            return newBlog;
        }

        public async Task<Blog> UpdateBlog(int blogId, string name)
        {
            var existingBlog = await _dbContext.Blogs.SingleOrDefaultAsync( b => b.BlogId == blogId);
            existingBlog.Name = name;
            await _dbContext.SaveChangesAsync();
            return existingBlog;
        }

        public async Task RemoveBlog(int blogId)
        {
            var existingBlog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync( b => b.BlogId == blogId);
            if (existingBlog == null)
            {
                return;
            }

            _dbContext.Remove(existingBlog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Blog>> GetAllBlogs() => await _dbContext.Blogs.ToArrayAsync();

        public async Task<Blog> GetBlog(int blogId) => 
            await _dbContext.Blogs
                .Include(b => b.BlogPosts)
                .Include(b => b.BlogAuthor)
                .Include(b => b.Theme)
                .SingleOrDefaultAsync(b => b.BlogId == blogId);

        public async Task<IEnumerable<Post>> GetAllBlogPosts(int blogId)
        {
            var blog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync(b => b.BlogId == blogId);
            return blog?.BlogPosts;
        }

        #endregion
        
        #region Author
        public async Task<Author> GetAuthor(int authorId) => await _dbContext.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId);
        
        public async Task<Author> GetAuthor(string email) => await _dbContext.Authors.SingleOrDefaultAsync(a => a.Email == email);

        public async Task<IEnumerable<Author>> GetAllAuthors() => await _dbContext.Authors.ToArrayAsync();
        
        public async Task<Author> AddAuthor(Author author)
        {
            var existingBlog = await _dbContext.Blogs.SingleOrDefaultAsync(b => b.BlogId == author.BlogId);
            existingBlog.BlogAuthor = author;
            await _dbContext.SaveChangesAsync();
            return author;
        }
        
        public async Task<Author> UpdateAuthorDetails(Author author)
        {
            var existingAuthor = await _dbContext.Authors.SingleOrDefaultAsync(a => a.Email == author.Email);
            existingAuthor.FirstName = author.FirstName;
            existingAuthor.LastName = author.LastName;
            
            await _dbContext.SaveChangesAsync();
            return existingAuthor;
        }

        public async Task RemoveAuthor(int authorId)
        {
            var existingAuthor = await _dbContext.Authors.SingleOrDefaultAsync(a => a.AuthorId == authorId);
            if (existingAuthor == null)
                return;
            var blog = await _dbContext.Blogs.SingleOrDefaultAsync(b => b.BlogId == existingAuthor.BlogId);
            blog.BlogAuthor = null;
            _dbContext.Authors.Remove(existingAuthor);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Theme
        public async Task<Theme> GetTheme(int themeId) => await _dbContext.Themes.SingleOrDefaultAsync(t => t.ThemeId == themeId);
        
        public async Task<Theme> GetTheme(string themeName) => await _dbContext.Themes.SingleOrDefaultAsync(t => t.ThemeName == themeName);

        public async Task<IEnumerable<Theme>> GetAllThemes() => await _dbContext.Themes.ToArrayAsync();

        public async Task<Theme> AddTheme(string themeName)
        {
            var newTheme = new Theme { ThemeName = themeName };
            await _dbContext.Themes.AddAsync(newTheme);
            await _dbContext.SaveChangesAsync();
            return newTheme;
        }
        
        public async Task<Theme> UpdateTheme(Theme theme)
        {
            var existingTheme = await _dbContext.Themes.SingleOrDefaultAsync(t => t.ThemeId == theme.ThemeId);
            existingTheme.ThemeName = theme.ThemeName;
            await _dbContext.SaveChangesAsync();
            return existingTheme;
        }
        
        public async Task RemoveTheme(int themeId)
        {
            var existingTheme = await _dbContext.Themes.SingleOrDefaultAsync(t => t.ThemeId == themeId);
            _dbContext.Themes.Remove(existingTheme);
            await _dbContext.SaveChangesAsync();
        }
        
        #endregion
    }
}