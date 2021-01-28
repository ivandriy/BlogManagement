using BlogManagement.Abstract;
using BlogManagement.DTO.Request;
using BlogManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        #region Post

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
        {
            var result = await _postService.GetAllPosts();
            return Ok(result);
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
        public async Task<ActionResult<Post>> AddPost([FromBody] CreateUpdatePost post)
        {
            if (post.BlogId != 0)
            {
                var existingBlog = await _blogService.GetBlog(post.BlogId);
                if(existingBlog == null) return BadRequest($"Blog with id {post.BlogId} is not exist");
            }
            var result = await _postService.AddNewPost(post);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> UpdatePost([FromRoute] int postId, [FromBody] CreateUpdatePost post)
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

        #endregion

        #region Category

        [HttpGet]
        [Route("Category")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var result = await _postService.GetAllCategories();
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Category/{categoryId}")]
        public async Task<ActionResult<Category>> GetCategory([FromRoute] int categoryId)
        {
            var result = await _postService.GetCategory(categoryId);
            if (result == null) return NotFound(categoryId);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("Category")]
        public async Task<ActionResult<Category>> AddCategory([FromQuery] string categoryName)
        {
            var existingCategory = await _postService.GetCategory(categoryName);
            if (existingCategory != null)
                return BadRequest($"Category with name {categoryName} already exists");
            var result = await _postService.AddCategory(categoryName);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("Category/{categoryId}")]
        public async Task<ActionResult<Category>> UpdateCategory([FromRoute] int categoryId, [FromQuery] string categoryName)
        {
            var existingCategory = await _postService.GetCategory(categoryId);
            if (existingCategory == null)
                return BadRequest($"Category with id {categoryId} doesn't exist");
            var result = await _postService.UpdateCategory(categoryId, categoryName);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Category/{categoryId}")]
        public async Task<ActionResult> RemoveCategory([FromRoute] int categoryId)
        {
            var existingCategory = await _postService.GetCategory(categoryId);
            if (existingCategory == null)
                return BadRequest($"Category with id {categoryId} doesn't exist");
            await _postService.RemoveCategory(categoryId);
            return Ok();
        }

        #endregion

    }
}