using BlogManagement.DataAccess.Abstract;
using BlogManagement.DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogManagement.DataAccess.DTO.Response;
using BlogManagement.Services;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogController : ControllerBase
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IBlogService _blogService;

        public BlogController(IBlogRepository blogRepository, IBlogService blogService)
        {
            _blogRepository = blogRepository;
            _blogService = blogService;
        }

        #region Blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            var result = await _blogRepository.GetAllBlogs();
            return Ok(result);
        }

        [HttpGet]
        [Route("{blogId}")]
        public async Task<ActionResult<Blog>> GetBlog(int blogId)
        {
            var result = await _blogRepository.GetBlog(blogId);
            if (result == null) return NotFound(blogId);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{blogId}/Posts")]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> GetBlogPosts([FromRoute]int blogId)
        {
            var result = await _blogService.GetBlogPosts(blogId);
            if (result == null) return NotFound($"Blog with id {blogId} is not exist");
            return Ok(result);
        }
        
        [HttpPost]
        [Route("{blogId}/Posts")]
        public async Task<ActionResult<IEnumerable<Post>>> AddPostsToBlog([FromRoute]int blogId, [FromBody]int[] postIds)
        {
            var existingBlog = await _blogRepository.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            await _blogRepository.AddPostsToBlog(blogId, postIds.ToArray());
            return Ok();
        }
        
        [HttpDelete]
        [Route("{blogId}/Posts")]
        public async Task<ActionResult<IEnumerable<Post>>> RemovePostsFromBlog([FromRoute]int blogId, [FromBody]int[] postIds)
        {
            var existingBlog = await _blogRepository.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            await _blogRepository.RemovePostsFromBlog(blogId, postIds.ToArray());
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> AddBlog([FromQuery] string blogName)
        {
            var result = await _blogRepository.AddBlog(blogName);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("{blogId}")]
        public async Task<ActionResult<Blog>> UpdateBlog([FromRoute]int blogId, [FromQuery]string blogName, [FromQuery]string themeName)
        {
            var existingBlog = await _blogRepository.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            var result = await _blogRepository.UpdateBlog(blogId, blogName, themeName);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("{blogId}")]
        public async Task<ActionResult> RemoveBlog([FromRoute]int blogId)
        {
            var existingBlog = await _blogRepository.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            await _blogRepository.RemoveBlog(blogId);
            return Ok();
        }
        
        #endregion
        
        #region Author
        [HttpPost]
        [Route("Author")]
        public async Task<ActionResult<Author>> AddAuthor([FromBody] Author newAuthor)
        {
            var existingBlog = await _blogRepository.GetBlog(newAuthor.BlogId);
            if (existingBlog == null)
                return BadRequest($"Blog with id {newAuthor.BlogId} doesn't exist");
            var existingAuthor = await _blogRepository.GetAuthor(newAuthor.Email);
            if (existingAuthor != null)
                return BadRequest($"Author with email {newAuthor.Email} already exist");
            var result = await _blogRepository.AddAuthor(newAuthor);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Author/{authorId}")]
        public async Task<ActionResult<Author>> GetAuthor([FromRoute] int authorId)
        {
            var result = await _blogRepository.GetAuthor(authorId);
            if (result == null)
                return NotFound(authorId);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Author")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthors()
        {
            var result = await _blogRepository.GetAllAuthors();
            return Ok(result);
        }
        
        [HttpPut]
        [Route("Author")]
        public async Task<ActionResult<Author>> UpdateAuthor([FromBody] Author updateAuthor)
        {
            var existingAuthor = await _blogRepository.GetAuthor(updateAuthor.AuthorId);
            if (existingAuthor == null)
                return BadRequest($"Author with id {updateAuthor.AuthorId} doesn't exist");
            var result = await _blogRepository.UpdateAuthorDetails(updateAuthor);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Author/{authorId}")]
        public async Task<ActionResult> RemoveAuthor([FromRoute] int authorId)
        {
            var existingAuthor = await _blogRepository.GetAuthor(authorId);
            if (existingAuthor == null)
                return BadRequest($"Author with id {authorId} doesn't exist");
            await _blogRepository.RemoveAuthor(authorId);
            return Ok();
        }

        #endregion

        #region Theme
        [HttpPost]
        [Route("Theme")]
        public async Task<ActionResult<Theme>> AddTheme([FromQuery] string themeName)
        {
            var existingTheme = await _blogRepository.GetTheme(themeName);
            if (existingTheme != null)
                return BadRequest($"Theme with name {themeName} already exists");
            var result = await _blogRepository.AddTheme(themeName);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Theme/{themeId}")]
        public async Task<ActionResult<Theme>> GetTheme([FromRoute] int themeId)
        {
            var result = await _blogRepository.GetTheme(themeId);
            if (result == null)
                return NotFound(themeId);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Theme")]
        public async Task<ActionResult<IEnumerable<Theme>>> GetAllThemes()
        {
            var result = await _blogRepository.GetAllThemes();
            return Ok(result);
        }
        
        [HttpPut]
        [Route("Theme/{themeId}")]
        public async Task<ActionResult<Theme>> UpdateTheme([FromRoute] int themeId, [FromQuery] string themeName)
        {
            var themeExists = await _blogRepository.GetTheme(themeId);
            if (themeExists == null)
                return BadRequest($"Theme with id {themeId} doesn't exist");
            var result = await _blogRepository.UpdateTheme(new Theme{ThemeId = themeId, ThemeName = themeName});
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Theme/{themeId}")]
        public async Task<ActionResult> RemoveTheme([FromRoute] int themeId)
        {
            var themeExists = await _blogRepository.GetTheme(themeId);
            if (themeExists == null)
                return BadRequest($"Theme with id {themeId} doesn't exist");
            await _blogRepository.RemoveTheme(themeId);
            return Ok();
        }
        
        #endregion
        
    }
}