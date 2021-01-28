using BlogManagement.Abstract;
using BlogManagement.DataAccess;
using BlogManagement.DTO.Request;
using BlogManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public async Task<Post> GetPost(int postId)
    {
        var post = await _dbContext.Posts.FindAsync(postId);
        await _dbContext.Entry(post).Collection(p => p.Categories).LoadAsync();
        return post;
    }

    public async Task<IEnumerable<Post>> GetAllPosts() => await _dbContext.Posts.ToArrayAsync();

    public async Task<Post> AddNewPost(CreateUpdatePost post)
    {
        var currDateTime = _clock.GetCurrentDateTime();
        var userName = string.Empty;
        var postCategories = new List<Category>();
        if (_httpContextAccessor.HttpContext != null)
        {
            userName = _httpContextAccessor.HttpContext.User.FindFirst(claim => claim.Type == ClaimTypes.Email)?.Value;
        }

        if (post.CategoryIds.Any())
        {
            postCategories = await _dbContext.Categories.Where(c => post.CategoryIds.Contains(c.CategoryId)).ToListAsync();
        }

        var newPost = new Post
        {
            Title = post.Title,
            Content = post.Body,
            CreatedOn = currDateTime,
            UpdatedOn = currDateTime,
            UserName = userName,
            Categories = postCategories
        };
        
        if (post.BlogId != 0)
        {
            var existingBlog = await _dbContext.Blogs.FindAsync(post.BlogId);
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

    public async Task<Post> UpdatePost(int postId, CreateUpdatePost post)
    {
        var postToUpdate = await _dbContext.Posts.FindAsync(postId);
        if(!string.IsNullOrWhiteSpace(post.Title))
            postToUpdate.Title = post.Title;
        if(!string.IsNullOrWhiteSpace(post.Body))
            postToUpdate.Content = post.Body;
        if (post.CategoryIds.Any())
        {
            var existingCategories = postToUpdate.Categories.Select(c => c.CategoryId).ToList();
            var categoryIdsToAdd = post.CategoryIds.Except(existingCategories).ToList();
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

    public async Task<Category> GetCategory(int categoryId)
    {
        var category = await _dbContext.Categories.FindAsync(categoryId);
        if (category != null)
        {
            await _dbContext.Entry(category).Collection(c => c.CategoryPosts).LoadAsync();
            
        }
        return category;
    }

    public async Task<Category> GetCategory(string categoryName) => await _dbContext.Categories.Include(c => c.CategoryPosts).SingleOrDefaultAsync(c => c.Name == categoryName);

    public async Task<IEnumerable<Category>> GetAllCategories() => await _dbContext.Categories.ToArrayAsync();

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