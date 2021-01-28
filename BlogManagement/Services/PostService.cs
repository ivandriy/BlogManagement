using BlogManagement.Abstract;
using BlogManagement.DataAccess;
using BlogManagement.DTO.Request;
using BlogManagement.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogManagement.Services
{
    public class PostService : IPostService

    {
    private readonly BlogDbContext _dbContext;
    private readonly ISystemClock _clock;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostService(BlogDbContext dbContext, ISystemClock clock, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _clock = clock;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Post> GetPost(int postId) => await _dbContext.Posts.FindAsync(postId);

    public async Task<Post> AddNewPost(CreatePost post)
    {
        var existingBlog = await _dbContext.Blogs.FindAsync(post.BlogId);
        await _dbContext.Entry(existingBlog).Collection(b => b.BlogPosts).LoadAsync();
        var currDateTime = _clock.GetCurrentDateTime();
        var userName = string.Empty;
        if (_httpContextAccessor.HttpContext != null)
        {
            userName = _httpContextAccessor.HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value;
        }

        var newPost = new Post
        {
            Title = post.Title,
            Content = post.Body,
            CreatedOn = currDateTime,
            UpdatedOn = currDateTime,
            UserName = userName
        };

        existingBlog.BlogPosts.Add(newPost);
        await _dbContext.SaveChangesAsync();
        return newPost;
    }

    public async Task RemovePost(int postId)
    {
        var postToRemove = await _dbContext.Posts.FindAsync(postId);
        _dbContext.Remove(postToRemove);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Post> UpdatePost(int postId, UpdatePost post)
    {
        var postToUpdate = await _dbContext.Posts.FindAsync(postId);
        postToUpdate.Title = post.Title;
        postToUpdate.Content = post.Body;
        postToUpdate.UpdatedOn = _clock.GetCurrentDateTime();
        await _dbContext.SaveChangesAsync();
        return postToUpdate;
    }
    }
}