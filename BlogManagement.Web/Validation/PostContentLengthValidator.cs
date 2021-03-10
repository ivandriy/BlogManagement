using System.Collections.Generic;
using System.Threading.Tasks;
using BlogManagement.DataAccess.DTO.Request;

namespace BlogManagement.Validation
{
    public class PostContentLengthValidator : IPostValidator
    {
        private readonly int _minContentLength = 100;
        public ValueTask<ValidationResult> Validate(UpdatePostRequest post)
        {
            var validationResult = new ValidationResult();
            if (post.Body.Length < _minContentLength)
            {
                validationResult.ErrorMessages = new List<string>()
                    {$"Post body content is less than {_minContentLength} symbols"};
            }

            return new ValueTask<ValidationResult>(validationResult);
        }
    }
}