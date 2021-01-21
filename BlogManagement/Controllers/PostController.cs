using BlogManagement.Abstract;
using BlogManagement.DTO;
using BlogManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController: ControllerBase
    {
        private readonly IBlogService _blogService;

        public PostController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> GetPost([FromRoute] int postId)
        {
            var result = await _blogService.GetPost(postId);
            if (result == null) return NotFound(postId);
            return Ok(result);
        }

        [HttpGet]
        [Route("Blog/{blogId}")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllBlogPosts([FromRoute]int blogId)
        {
            var result = await _blogService.GetAllBlogPosts(blogId);
            if (result == null) return NotFound(blogId);
            return Ok(result);
        }

        [HttpPost]
        public async Task AddPost([FromBody] CreateUpdatePost post)
        {
            await _blogService.AddNewPost(post.Title, post.Body, post.BlogId);
        }
        
        [HttpPut]
        [Route("{postId}")]
        public async Task UpdatePost([FromRoute] int postId, [FromBody] CreateUpdatePost post)
        {
            await _blogService.UpdatePost(postId, post.Title, post.Body);
        }
        
        [HttpDelete]
        [Route("{postId}")]
        public async Task RemovePost([FromRoute] int postId)
        {
            await _blogService.RemovePost(postId);
        }
    }
}