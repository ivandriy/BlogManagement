using BlogManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogManagement.Abstract
{
    public interface IBlogService
    {
        Task<Blog> AddBlog(string name);
        
        Task<Blog> UpdateBlog(int blogId, string name);
        
        Task RemoveBlog(int blogId);

        Task<IEnumerable<Blog>> GetAllBlogs();
        
        Task<Blog> GetBlog(int blogId);

        Task<IEnumerable<Post>> GetAllBlogPosts(int blogId);

        Task<IEnumerable<Author>> GetAllAuthors();
        
        Task<Author> GetAuthor(int authorId);

        Task<Author> GetAuthor(string email);

        Task<Author> AddAuthor(Author author);

        Task<Author> UpdateAuthorDetails(Author author);

        Task RemoveAuthor(int authorId);

        Task<Theme> GetTheme(int themeId);
        
        Task<Theme> GetTheme(string themeName);

        Task<IEnumerable<Theme>> GetAllThemes();

        Task<Theme> AddTheme(string themeName);

        Task<Theme> UpdateTheme(Theme theme);

        Task RemoveTheme(int themeId);
    }
}