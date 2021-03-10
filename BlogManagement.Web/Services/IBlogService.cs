using System.Collections.Generic;
using System.Threading.Tasks;
using BlogManagement.DataAccess.DTO.Response;

namespace BlogManagement.Services
{
    public interface IBlogService
    {
        Task<IEnumerable<PostViewModel>> GetBlogPosts(int blogId);
    }
}