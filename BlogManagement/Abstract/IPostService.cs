using BlogManagement.DTO.Request;
using BlogManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Abstract
{
    public interface IPostService
    {
        Task<Post> GetPost(int postId);

        Task<IEnumerable<Post>> GetAllPosts();
        
        Task<Post> AddNewPost(CreateUpdatePost post);
        
        Task RemovePost(int postId);
        
        Task<Post> UpdatePost(int postId, CreateUpdatePost postUpd);

        Task<Category> GetCategory(int categoryId);

        Task<Category> GetCategory(string categoryName);

        Task<IEnumerable<Category>> GetAllCategories();

        Task<Category> AddCategory(string categoryName);

        Task<Category> UpdateCategory(int categoryId, string categoryName);

        Task RemoveCategory(int categoryId);
    }
}