using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogManagement.DataAccess.DTO.Request;

namespace BlogManagement.Validation
{
    public class DefaultPostValidationProcessor : IPostValidationProcessor
    {
        private readonly IEnumerable<IPostValidator> _validators;

        public DefaultPostValidationProcessor(IEnumerable<IPostValidator> validators)
        {
            _validators = validators;
        }
        public async ValueTask<ValidationResult> ValidateAll(UpdatePostRequest post)
        {
            var validationResult = new ValidationResult();
            var validationResults = await Task.WhenAll(_validators.Select(async v => await v.Validate(post)));
            if (validationResults.Any(v => v.IsSuccessful != true))
            {
                validationResult.ErrorMessages = validationResults.Where(v => v.IsSuccessful != true)
                    .SelectMany(v => v.ErrorMessages).ToList();
            }

            return validationResult;
        }
    }
}