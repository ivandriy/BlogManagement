using BlogManagement.DataAccess.Abstract;
using BlogManagement.DataAccess.DTO.Request;
using BlogManagement.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogManagement.Validation;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController: ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly IBlogRepository _blogRepository;
        private readonly IPostValidationProcessor _validator;

        public PostController(IPostRepository postRepository, IBlogRepository blogRepository, IPostValidationProcessor validator)
        {
            _postRepository = postRepository;
            _blogRepository = blogRepository;
            _validator = validator;
        }

        #region Post

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
        {
            var result = await _postRepository.GetAllPosts();
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> GetPost([FromRoute] int postId)
        {
            var result = await _postRepository.GetPost(postId);
            if (result == null) return NotFound(postId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] CreatePostRequest post)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var existingBlog = await _blogRepository.GetBlog(post.BlogId);
            if(existingBlog == null) return BadRequest($"Blog with id {post.BlogId} is not exist");
            var validationResult = await _validator.ValidateAll(post);
            if (!validationResult.IsSuccessful)
                return BadRequest(validationResult.ErrorMessages);
            var result = await _postRepository.AddNewPost(post);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("{postId}")]
        public async Task<ActionResult<Post>> UpdatePost([FromRoute] int postId, [FromBody] UpdatePostRequest post)
        {
            var existingPost = await _postRepository.GetPost(postId);
            if(existingPost == null) return BadRequest($"Post with id {postId} is not exist");
            var result = await _postRepository.UpdatePost(postId, post);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("{postId}")]
        public async Task<ActionResult> RemovePost([FromRoute] int postId)
        {
            var existingPost = await _postRepository.GetPost(postId);
            if(existingPost == null) return BadRequest($"Post with id {postId} is not exist");
            await _postRepository.RemovePost(postId);
            return Ok();
        }

        #endregion

        #region Category

        [HttpGet]
        [Route("Category")]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var result = await _postRepository.GetAllCategories();
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Category/{categoryId}")]
        public async Task<ActionResult<Category>> GetCategory([FromRoute] int categoryId)
        {
            var result = await _postRepository.GetCategory(categoryId);
            if (result == null) return NotFound(categoryId);
            return Ok(result);
        }
        
        [HttpPost]
        [Route("Category")]
        public async Task<ActionResult<Category>> AddCategory([FromQuery] string categoryName)
        {
            var existingCategory = await _postRepository.GetCategory(categoryName);
            if (existingCategory != null)
                return BadRequest($"Category with name {categoryName} already exists");
            var result = await _postRepository.AddCategory(categoryName);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("Category/{categoryId}")]
        public async Task<ActionResult<Category>> UpdateCategory([FromRoute] int categoryId, [FromQuery] string categoryName)
        {
            var existingCategory = await _postRepository.GetCategory(categoryId);
            if (existingCategory == null)
                return BadRequest($"Category with id {categoryId} doesn't exist");
            var result = await _postRepository.UpdateCategory(categoryId, categoryName);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Category/{categoryId}")]
        public async Task<ActionResult> RemoveCategory([FromRoute] int categoryId)
        {
            var existingCategory = await _postRepository.GetCategory(categoryId);
            if (existingCategory == null)
                return BadRequest($"Category with id {categoryId} doesn't exist");
            await _postRepository.RemoveCategory(categoryId);
            return Ok();
        }

        #endregion

    }
}