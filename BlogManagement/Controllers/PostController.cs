using BlogManagement.Abstract;
using BlogManagement.DTO.Request;
using BlogManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController: ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IBlogService _blogService;

        public PostController(IPostService postService, IBlogService blogService)
        {
            _postService = postService;
            _blogService = blogService;
        }

        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> GetPost([FromRoute] int postId)
        {
            var result = await _postService.GetPost(postId);
            if (result == null) return NotFound(postId);
            return Ok(result);
        }



        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] CreatePost post)
        {
            var existingBlog = await _blogService.GetBlog(post.BlogId);
            if(existingBlog == null) return BadRequest($"Blog with id {post.BlogId} is not exist");
            var result = await _postService.AddNewPost(post);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> UpdatePost([FromRoute] int postId, [FromBody] UpdatePost post)
        {
            var existingPost = await _postService.GetPost(postId);
            if(existingPost == null) return BadRequest($"Post with id {postId} is not exist");
            var result = await _postService.UpdatePost(postId, post);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("{postId}")]
        public async Task<ActionResult> RemovePost([FromRoute] int postId)
        {
            var existingPost = await _postService.GetPost(postId);
            if(existingPost == null) return BadRequest($"Post with id {postId} is not exist");
            await _postService.RemovePost(postId);
            return Ok();
        }
    }
}