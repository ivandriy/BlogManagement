using BlogManagement.DataAccess.Abstract;
using BlogManagement.DataAccess.DTO.Request;
using BlogManagement.DataAccess.Models;
using BlogManagement.Infrastructure.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogManagement.DataAccess.Repositories
{
    public class PostRepository : IPostRepository

    {
    private readonly BlogDbContext _dbContext;
    private readonly ISystemClock _clock;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PostRepository(BlogDbContext dbContext, ISystemClock clock, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _clock = clock;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Post> GetPost(int postId) => 
        await _dbContext.Posts.Include(p => p.Categories).AsNoTracking().SingleOrDefaultAsync(p => p.PostId == postId);

    public async Task<IEnumerable<Post>> GetAllPosts() => await _dbContext.Posts.AsNoTracking().ToArrayAsync();

    public async Task<Post> AddNewPost(CreatePostRequest createPost)
    {
        var currDateTime = _clock.GetCurrentDateTime();
        var userName = string.Empty;
        var postCategories = new List<Category>();
        if (_httpContextAccessor.HttpContext != null)
        {
            userName = _httpContextAccessor.HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value;
        }

        if (createPost.CategoryIds.Any())
        {
            postCategories = await _dbContext.Categories.Where(c => createPost.CategoryIds.Contains(c.CategoryId)).ToListAsync();
        }

        var newPost = new Post
        {
            Title = createPost.Title,
            Content = createPost.Body,
            CreatedOn = currDateTime,
            UpdatedOn = currDateTime,
            UserName = userName,
            Categories = postCategories,
            BlogId = createPost.BlogId
        };
        
        if (createPost.BlogId != 0)
        {
            var existingBlog = await _dbContext.Blogs.FindAsync(createPost.BlogId);
            await _dbContext.Entry(existingBlog).Collection(b => b.BlogPosts).LoadAsync();
            existingBlog.BlogPosts.Add(newPost);
        }
        else
        {
            await _dbContext.Posts.AddAsync(newPost);
        }
        
        await _dbContext.SaveChangesAsync();
        return newPost;
    }

    public async Task RemovePost(int postId)
    {
        var postToRemove = await _dbContext.Posts.FindAsync(postId);
        _dbContext.Remove(postToRemove);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Post> UpdatePost(int postId, UpdatePostRequest updatePost)
    {
        var postToUpdate = await _dbContext.Posts.Include(p => p.Categories).SingleOrDefaultAsync(p => p.PostId == postId);
        if(!string.IsNullOrWhiteSpace(updatePost.Title))
            postToUpdate.Title = updatePost.Title;
        if(!string.IsNullOrWhiteSpace(updatePost.Body))
            postToUpdate.Content = updatePost.Body;
        if (updatePost.CategoryIds.Any())
        {
            var existingCategories = postToUpdate.Categories.Select(c => c.CategoryId).ToList();
            var categoryIdsToAdd = updatePost.CategoryIds.Except(existingCategories).ToList();
            if (categoryIdsToAdd.Any())
            {
                var categoriesToAdd = await _dbContext.Categories.Where(c => categoryIdsToAdd.Contains(c.CategoryId)).ToListAsync();
                postToUpdate.Categories.AddRange(categoriesToAdd);
                _dbContext.Entry(postToUpdate).State = EntityState.Modified;
            }
        }
        if (_dbContext.Entry(postToUpdate).State == EntityState.Modified)
        {
            postToUpdate.UpdatedOn = _clock.GetCurrentDateTime();
            await _dbContext.SaveChangesAsync();
        }
        return postToUpdate;
    }

    public async Task<Category> GetCategory(int categoryId) =>
        await _dbContext.Categories.Include(c => c.CategoryPosts)
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.CategoryId == categoryId);

    public async Task<Category> GetCategory(string categoryName) =>
        await _dbContext.Categories.Include(c => c.CategoryPosts)
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.Name == categoryName);

    public async Task<IEnumerable<Category>> GetAllCategories() => await _dbContext.Categories.AsNoTracking().ToArrayAsync();

    public async Task<Category> AddCategory(string categoryName)
    {
        var newCategory = new Category
        {
            Name = categoryName
        };
        await _dbContext.Categories.AddAsync(newCategory);
        await _dbContext.SaveChangesAsync();
        return newCategory;
    }

    public async Task<Category> UpdateCategory(int categoryId, string categoryName)
    {
        var existingCategory = await _dbContext.Categories.FindAsync(categoryId);
        existingCategory.Name = categoryName;
        await _dbContext.SaveChangesAsync();
        return existingCategory;
    }
    
    public async Task RemoveCategory(int categoryId)
    {
        var existingCategory = await _dbContext.Categories.FindAsync(categoryId);
        _dbContext.Categories.Remove(existingCategory);
        await _dbContext.SaveChangesAsync();
    }

    }
}