using BlogManagement.Abstract;
using BlogManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public AdminController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [Route("Blog")]
        public async Task<IEnumerable<Blog>> GetBlogs() => await _blogService.GetAllBlogs();
        
        [HttpGet]
        [Route("Blog/{blogId}")]
        public async Task<Blog> GetBlog(int blogId) => await _blogService.GetBlog(blogId);

        [HttpPost]
        [Route("Blog")]
        public async Task AddBlog([FromQuery] string blogName)
        {
            await _blogService.AddBlog(blogName);
        }
        
        [HttpPut]
        [Route("Blog/{blogId}")]
        public async Task UpdateBlog([FromRoute]int blogId, [FromQuery]string blogName)
        {
            await _blogService.UpdateBlog(blogId, blogName);
        }
        
        [HttpDelete]
        [Route("Blog/{blogId}")]
        public async Task RemoveBlog([FromRoute]int blogId)
        {
            await _blogService.RemoveBlog(blogId);
        }
    }
}