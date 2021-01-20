using BlogManagement.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Abstract
{
    public interface IBlogService
    {
        Task AddBlog(string name);
        Task UpdateBlog(int blogId, string name);
        Task RemoveBlog(int blogId);

        Task<IEnumerable<Blog>> GetAllBlogs();
        
        Task<Blog> GetBlog(int blogId);

        Task AddNewPost(string title, string body, int blogId);
        Task RemovePost(int postId, int blogId);
        Task UpdatePost(int postId, int blogId, string title, string body);
    }
}