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
        
        public async Task AddBlog(string name)
        {
            await _dbContext.Blogs.AddAsync(new Blog { Name = name });
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBlog(int blogId, string name)
        {
            var existingBlog = await _dbContext.Blogs.SingleOrDefaultAsync( b => b.Id == blogId);
            if (existingBlog == null)
            {
                return;
            }

            existingBlog.Name = name;
            await _dbContext.SaveChangesAsync();

        }

        public async Task RemoveBlog(int blogId)
        {
            var existingBlog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync( b => b.Id == blogId);
            if (existingBlog == null)
            {
                return;
            }

            _dbContext.Remove(existingBlog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Blog>> GetAllBlogs() => await _dbContext.Blogs.ToArrayAsync();

        public async Task<Blog> GetBlog(int blogId) => await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync(b => b.Id == blogId);

        public async Task AddNewPost(string title, string body, int blogId)
        {
            var existingBlog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync( b => b.Id == blogId);
            if (existingBlog == null)
            {
                return;
            }

            var newPost = new Post { Title = title, Content = body };

           existingBlog.BlogPosts.Add(newPost);
           await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePost(int postId, int blogId)
        {
            var existingBlog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync( b => b.Id == blogId);
            if (existingBlog == null)
            {
                return;
            }
            var postToRemove = existingBlog.BlogPosts.SingleOrDefault(p => p.Id == postId);
            if (postToRemove == null)
            {
                return;
            }
            existingBlog.BlogPosts.Remove(postToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePost(int postId, int blogId, string title, string body)
        {
            var existingBlog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync( b => b.Id == blogId);
            if (existingBlog == null)
            {
                return;
            }
            
            var postToUpdate = existingBlog.BlogPosts.SingleOrDefault(p => p.Id == postId);
            if (postToUpdate ==  null)
            {
                return;
            }

            postToUpdate.Title = title;
            postToUpdate.Content = body;
            await _dbContext.SaveChangesAsync();

        }
    }
}