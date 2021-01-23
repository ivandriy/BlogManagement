using BlogManagement.DTO.Request;
using BlogManagement.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Abstract
{
    public interface IBlogService
    {
        Task<Blog> AddBlog(string name);
        Task<Blog> UpdateBlog(int blogId, string name);
        Task RemoveBlog(int blogId);

        Task<IEnumerable<Blog>> GetAllBlogs();
        
        Task<Blog> GetBlog(int blogId);

        Task<IEnumerable<Post>> GetAllBlogPosts(int blogId);

        Task<Post> GetPost(int postId);
        
        Task<Post> AddNewPost(CreatePost post);
        Task RemovePost(int postId);
        Task<Post> UpdatePost(int postId, UpdatePost postUpd);
    }
}