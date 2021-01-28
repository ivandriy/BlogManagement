using BlogManagement.DTO.Request;
using BlogManagement.Models;
using System.Threading.Tasks;

namespace BlogManagement.Abstract
{
    public interface IPostService
    {
        Task<Post> GetPost(int postId);
        
        Task<Post> AddNewPost(CreatePost post);
        
        Task RemovePost(int postId);
        
        Task<Post> UpdatePost(int postId, UpdatePost postUpd);
    }
}