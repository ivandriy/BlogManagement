using BlogManagement.Abstract;
using BlogManagement.DataAccess;
using BlogManagement.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<Blog> UpdateBlog(int blogId, string name, string themeName)
        {
            var existingBlog = await _dbContext.Blogs.FindAsync(blogId);
            if(!string.IsNullOrWhiteSpace(name))
                existingBlog.Name = name;
            if(!string.IsNullOrWhiteSpace(themeName))
                existingBlog.Theme = await GetTheme(themeName);
            await _dbContext.SaveChangesAsync();
            return existingBlog;
        }

        public async Task RemoveBlog(int blogId)
        {
            var existingBlog = await _dbContext.Blogs.FindAsync(blogId);
            _dbContext.Blogs.Remove(existingBlog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Blog>> GetAllBlogs() => await _dbContext.Blogs.ToArrayAsync();

        public async Task<Blog> GetBlog(int blogId)
        {
            var blog = await _dbContext.Blogs.FindAsync(blogId);
            await _dbContext.Entry(blog).Collection(b => b.BlogPosts).LoadAsync();
            await _dbContext.Entry(blog).Reference(b => b.BlogAuthor).LoadAsync();
            await _dbContext.Entry(blog).Reference(b => b.Theme).LoadAsync();
            return blog;
        }

        public async Task<IEnumerable<Post>> GetAllBlogPosts(int blogId)
        {
            var blog = await _dbContext.Blogs.FindAsync(blogId);
            await _dbContext.Entry(blog).Collection(b => b.BlogPosts).LoadAsync();
            return blog?.BlogPosts;
        }

        public async Task AddPostsToBlog(int blogId, IEnumerable<int> postIds)
        {
            var blog = await _dbContext.Blogs.FindAsync(blogId);
            await _dbContext.Entry(blog).Collection(b => b.BlogPosts).LoadAsync();

            var newPostIdsList = postIds.ToList();
            var postIdsToAdd = newPostIdsList.Except(blog.BlogPosts.Select(b => b.PostId));
            
            var postsToAdd = await _dbContext.Posts.Where(p => postIdsToAdd.Contains(p.PostId)).ToListAsync();
            blog.BlogPosts.AddRange(postsToAdd);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task RemovePostsFromBlog(int blogId, IEnumerable<int> postIds)
        {
            var blog = await _dbContext.Blogs.FindAsync(blogId);
            await _dbContext.Entry(blog).Collection(b => b.BlogPosts).LoadAsync();
            blog.BlogPosts.RemoveAll(p => postIds.Contains(p.PostId));
            await _dbContext.SaveChangesAsync();
        }

        #endregion
        
        #region Author

        public async Task<Author> GetAuthor(int authorId) => await _dbContext.Authors.FindAsync(authorId);
        
        public async Task<Author> GetAuthor(string email) => await _dbContext.Authors.SingleOrDefaultAsync(a => a.Email == email);

        public async Task<IEnumerable<Author>> GetAllAuthors() => await _dbContext.Authors.ToArrayAsync();
        
        public async Task<Author> AddAuthor(Author author)
        {
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return author;
        }
        
        public async Task<Author> UpdateAuthorDetails(Author author)
        {
            var existingAuthor = await _dbContext.Authors.FindAsync(author.AuthorId);
            existingAuthor.FirstName = author.FirstName;
            existingAuthor.LastName = author.LastName;
            
            await _dbContext.SaveChangesAsync();
            return existingAuthor;
        }

        public async Task RemoveAuthor(int authorId)
        {
            var existingAuthor = await _dbContext.Authors.FindAsync(authorId);
            var blog = await _dbContext.Blogs.FindAsync(existingAuthor.BlogId);
            await _dbContext.Entry(blog).Reference(b => b.BlogAuthor).LoadAsync();
            blog.BlogAuthor = null;
            _dbContext.Authors.Remove(existingAuthor);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region Theme
        public async Task<Theme> GetTheme(int themeId) => await _dbContext.Themes.FindAsync(themeId);
        
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
            var existingTheme = await _dbContext.Themes.FindAsync(theme.ThemeId);
            existingTheme.ThemeName = theme.ThemeName;
            await _dbContext.SaveChangesAsync();
            return existingTheme;
        }
        
        public async Task RemoveTheme(int themeId)
        {
            var existingTheme = await _dbContext.Themes.FindAsync(themeId);
            _dbContext.Themes.Remove(existingTheme);
            await _dbContext.SaveChangesAsync();
        }
        
        #endregion
    }
}