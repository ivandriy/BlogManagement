using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogManagement.DataAccess;
using BlogManagement.DataAccess.DTO.Request;
using Microsoft.EntityFrameworkCore;

namespace BlogManagement.Validation
{
    public class DuplicatePostTitleValidator : IPostValidator
    {
        private readonly BlogDbContext _dbContext;

        public DuplicatePostTitleValidator(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async ValueTask<ValidationResult> Validate(UpdatePostRequest post)
        {
            var postWithSameTitle = await _dbContext.Posts.Where(p => p.Title.ToUpper() == post.Title.ToUpper())
                .SingleOrDefaultAsync();
            return postWithSameTitle != null
                ? new ValidationResult
                {
                    ErrorMessages = new List<string>
                        {$"Post with the same title already exists - postId: {postWithSameTitle.PostId}"}
                }
                : new ValidationResult();
        }
    }
}