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
        private readonly ISystemClock _clock;

        public BlogService(BlogDbContext dbContext, ISystemClock clock)
        {
            _dbContext = dbContext;
            _clock = clock;
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

        public async Task<IEnumerable<Post>> GetAllBlogPosts(int blogId)
        {
            var blog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync(b => b.Id == blogId);
            return blog?.BlogPosts;
        }

        public async Task<Post> GetPost(int postId) => await _dbContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);

        public async Task AddNewPost(string title, string body, int blogId)
        {
            var existingBlog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync( b => b.Id == blogId);
            if (existingBlog == null)
            {
                return;
            }

            var currDateTime = _clock.GetCurrentDateTime();
            var newPost = new Post
            {
                Title = title,
                Content = body,
                CreatedOn = currDateTime,
                UpdatedOn = currDateTime
            };

           existingBlog.BlogPosts.Add(newPost);
           await _dbContext.SaveChangesAsync();
        }

        public async Task RemovePost(int postId)
        {
            var postToRemove = await _dbContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            if (postToRemove == null)
            {
                return;
            }

            _dbContext.Remove(postToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePost(int postId, string title, string body)
        {
            var postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(p => p.Id == postId);
            if (postToUpdate ==  null)
            {
                return;
            }

            postToUpdate.Title = title;
            postToUpdate.Content = body;
            postToUpdate.UpdatedOn = _clock.GetCurrentDateTime();
            await _dbContext.SaveChangesAsync();

        }
    }
}