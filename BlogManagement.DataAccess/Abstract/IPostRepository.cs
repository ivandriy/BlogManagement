using BlogManagement.DataAccess.DTO.Request;
using BlogManagement.DataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.DataAccess.Abstract
{
    public interface IPostRepository
    {
        Task<Post> GetPost(int postId);

        Task<IEnumerable<Post>> GetAllPosts();
        
        Task<Post> AddNewPost(CreatePostRequest createPost);
        
        Task RemovePost(int postId);
        
        Task<Post> UpdatePost(int postId, UpdatePostRequest postUpd);

        Task<Category> GetCategory(int categoryId);

        Task<Category> GetCategory(string categoryName);

        Task<IEnumerable<Category>> GetAllCategories();

        Task<Category> AddCategory(string categoryName);

        Task<Category> UpdateCategory(int categoryId, string categoryName);

        Task RemoveCategory(int categoryId);
    }
}