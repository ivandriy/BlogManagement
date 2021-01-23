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
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            var result = await _blogService.GetAllBlogs();
            return Ok(result);
        }

        [HttpGet]
        [Route("Blog/{blogId}")]
        public async Task<ActionResult<Blog>> GetBlog(int blogId)
        {
            var result = await _blogService.GetBlog(blogId);
            if (result == null) return NotFound(blogId);
            return Ok(result);
        }

        [HttpPost]
        [Route("Blog")]
        public async Task<ActionResult<Blog>> AddBlog([FromQuery] string blogName)
        {
            var result = await _blogService.AddBlog(blogName);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("Blog/{blogId}")]
        public async Task<ActionResult<Blog>> UpdateBlog([FromRoute]int blogId, [FromQuery]string blogName)
        {
            var existingBlog = await _blogService.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            var result = await _blogService.UpdateBlog(blogId, blogName);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Blog/{blogId}")]
        public async Task<ActionResult> RemoveBlog([FromRoute]int blogId)
        {
            var existingBlog = await _blogService.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            await _blogService.RemoveBlog(blogId);
            return Ok();
        }
    }
}