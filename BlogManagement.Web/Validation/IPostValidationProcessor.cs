using System.Threading.Tasks;
using BlogManagement.DataAccess.DTO.Request;

namespace BlogManagement.Validation
{
    public interface IPostValidationProcessor
    {
        ValueTask<ValidationResult> ValidateAll(UpdatePostRequest post);
    }
}