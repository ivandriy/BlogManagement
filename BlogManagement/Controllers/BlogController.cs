using BlogManagement.Abstract;
using BlogManagement.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        #region Blog
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
        {
            var result = await _blogService.GetAllBlogs();
            return Ok(result);
        }

        [HttpGet]
        [Route("{blogId}")]
        public async Task<ActionResult<Blog>> GetBlog(int blogId)
        {
            var result = await _blogService.GetBlog(blogId);
            if (result == null) return NotFound(blogId);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("{blogId}/Posts")]
        public async Task<ActionResult<IEnumerable<Post>>> GetBlogPosts([FromRoute]int blogId)
        {
            var result = await _blogService.GetAllBlogPosts(blogId);
            if (result == null) return NotFound($"Blog with id {blogId} is not exist");
            return Ok(result);
        }
        
        [HttpPost]
        [Route("{blogId}/Posts")]
        public async Task<ActionResult<IEnumerable<Post>>> AddPostsToBlog([FromRoute]int blogId, [FromBody]int[] postIds)
        {
            var existingBlog = await _blogService.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            await _blogService.AddPostsToBlog(blogId, postIds.ToArray());
            return Ok();
        }
        
        [HttpDelete]
        [Route("{blogId}/Posts")]
        public async Task<ActionResult<IEnumerable<Post>>> RemovePostsFromBlog([FromRoute]int blogId, [FromBody]int[] postIds)
        {
            var existingBlog = await _blogService.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            await _blogService.RemovePostsFromBlog(blogId, postIds.ToArray());
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Blog>> AddBlog([FromQuery] string blogName)
        {
            var result = await _blogService.AddBlog(blogName);
            return Ok(result);
        }
        
        [HttpPut]
        [Route("{blogId}")]
        public async Task<ActionResult<Blog>> UpdateBlog([FromRoute]int blogId, [FromQuery]string blogName, [FromQuery]string themeName)
        {
            var existingBlog = await _blogService.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            var result = await _blogService.UpdateBlog(blogId, blogName, themeName);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("{blogId}")]
        public async Task<ActionResult> RemoveBlog([FromRoute]int blogId)
        {
            var existingBlog = await _blogService.GetBlog(blogId);
            if (existingBlog == null) return BadRequest($"Blog with id {blogId} is not exist");
            await _blogService.RemoveBlog(blogId);
            return Ok();
        }
        
        #endregion
        
        #region Author
        [HttpPost]
        [Route("Author")]
        public async Task<ActionResult<Author>> AddAuthor([FromBody] Author newAuthor)
        {
            var existingBlog = await _blogService.GetBlog(newAuthor.BlogId);
            if (existingBlog == null)
                return BadRequest($"Blog with id {newAuthor.BlogId} doesn't exist");
            var existingAuthor = await _blogService.GetAuthor(newAuthor.Email);
            if (existingAuthor != null)
                return BadRequest($"Author with email {newAuthor.Email} already exist");
            var result = await _blogService.AddAuthor(newAuthor);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Author/{authorId}")]
        public async Task<ActionResult<Author>> GetAuthor([FromRoute] int authorId)
        {
            var result = await _blogService.GetAuthor(authorId);
            if (result == null)
                return NotFound(authorId);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Author")]
        public async Task<ActionResult<IEnumerable<Author>>> GetAllAuthors()
        {
            var result = await _blogService.GetAllAuthors();
            return Ok(result);
        }
        
        [HttpPut]
        [Route("Author")]
        public async Task<ActionResult<Author>> UpdateAuthor([FromBody] Author updateAuthor)
        {
            var existingAuthor = await _blogService.GetAuthor(updateAuthor.AuthorId);
            if (existingAuthor == null)
                return BadRequest($"Author with id {updateAuthor.AuthorId} doesn't exist");
            var result = await _blogService.UpdateAuthorDetails(updateAuthor);
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Author/{authorId}")]
        public async Task<ActionResult> RemoveAuthor([FromRoute] int authorId)
        {
            var existingAuthor = await _blogService.GetAuthor(authorId);
            if (existingAuthor == null)
                return BadRequest($"Author with id {authorId} doesn't exist");
            await _blogService.RemoveAuthor(authorId);
            return Ok();
        }

        #endregion

        #region Theme
        [HttpPost]
        [Route("Theme")]
        public async Task<ActionResult<Theme>> AddTheme([FromQuery] string themeName)
        {
            var existingTheme = await _blogService.GetTheme(themeName);
            if (existingTheme != null)
                return BadRequest($"Theme with name {themeName} already exists");
            var result = await _blogService.AddTheme(themeName);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Theme/{themeId}")]
        public async Task<ActionResult<Theme>> GetTheme([FromRoute] int themeId)
        {
            var result = await _blogService.GetTheme(themeId);
            if (result == null)
                return NotFound(themeId);
            return Ok(result);
        }
        
        [HttpGet]
        [Route("Theme")]
        public async Task<ActionResult<IEnumerable<Theme>>> GetAllThemes()
        {
            var result = await _blogService.GetAllThemes();
            return Ok(result);
        }
        
        [HttpPut]
        [Route("Theme/{themeId}")]
        public async Task<ActionResult<Theme>> UpdateTheme([FromRoute] int themeId, [FromQuery] string themeName)
        {
            var themeExists = await _blogService.GetTheme(themeId);
            if (themeExists == null)
                return BadRequest($"Theme with id {themeId} doesn't exist");
            var result = await _blogService.UpdateTheme(new Theme{ThemeId = themeId, ThemeName = themeName});
            return Ok(result);
        }
        
        [HttpDelete]
        [Route("Theme/{themeId}")]
        public async Task<ActionResult> RemoveTheme([FromRoute] int themeId)
        {
            var themeExists = await _blogService.GetTheme(themeId);
            if (themeExists == null)
                return BadRequest($"Theme with id {themeId} doesn't exist");
            await _blogService.RemoveTheme(themeId);
            return Ok();
        }
        
        #endregion
        
    }
}