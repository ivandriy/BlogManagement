using BlogManagement.Abstract;
using BlogManagement.DataAccess;
using BlogManagement.DTO.Request;
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

        public async Task<Blog> GetBlog(int blogId) => await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync(b => b.BlogId == blogId);

        public async Task<IEnumerable<Post>> GetAllBlogPosts(int blogId)
        {
            var blog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync(b => b.BlogId == blogId);
            return blog?.BlogPosts;
        }

        public async Task<Post> GetPost(int postId) => await _dbContext.Posts.SingleOrDefaultAsync(p => p.PostId == postId);

        public async Task<Post> AddNewPost(CreatePost post)
        {
            var existingBlog = await _dbContext.Blogs.Include(b => b.BlogPosts).SingleOrDefaultAsync( b => b.BlogId == post.BlogId);
            var existingUser = await _dbContext.Users.Include(u => u.Posts).SingleOrDefaultAsync(u => u.UserId == post.UserId);
            var currDateTime = _clock.GetCurrentDateTime();
            var newPost = new Post
            {
                Title = post.Title,
                Content = post.Body,
                CreatedOn = currDateTime,
                UpdatedOn = currDateTime,
                UserId = post.UserId,
                User = existingUser
            };

           existingBlog.BlogPosts.Add(newPost);
           existingUser.Posts.Add(newPost);
           await _dbContext.SaveChangesAsync();
           return newPost;
        }

        public async Task RemovePost(int postId)
        {
            var postToRemove = await _dbContext.Posts.SingleOrDefaultAsync(p => p.PostId == postId);
            _dbContext.Remove(postToRemove);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Post> UpdatePost(int postId, UpdatePost post)
        {
            var postToUpdate = await _dbContext.Posts.SingleOrDefaultAsync(p => p.PostId == postId);
            postToUpdate.Title = post.Title;
            postToUpdate.Content = post.Body;
            postToUpdate.UpdatedOn = _clock.GetCurrentDateTime();
            await _dbContext.SaveChangesAsync();
            return postToUpdate;
        }
    }
}