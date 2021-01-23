using BlogManagement.Abstract;
using BlogManagement.DTO.Request;
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
            if (result == null) return NotFound($"Blog with id {blogId} is not exist");
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] CreatePost post)
        {
            var existingBlog = await _blogService.GetBlog(post.BlogId);
            if(existingBlog == null) return BadRequest($"Blog with id {post.BlogId} is not exist");
            var result = await _blogService.AddNewPost(post);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> UpdatePost([FromRoute] int postId, [FromBody] UpdatePost post)
        {
            var existingPost = await _blogService.GetPost(postId);
            if(existingPost == null) return BadRequest($"Post with id {postId} is not exist");
            var result = await _blogService.UpdatePost(postId, post);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("{postId}")]
        public async Task<ActionResult> RemovePost([FromRoute] int postId)
        {
            var existingPost = await _blogService.GetPost(postId);
            if(existingPost == null) return BadRequest($"Post with id {postId} is not exist");
            await _blogService.RemovePost(postId);
            return Ok();
        }
    }
}