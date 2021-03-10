using System.Threading.Tasks;
using BlogManagement.DataAccess.DTO.Request;

namespace BlogManagement.Validation
{
    public interface IPostValidator
    {
        ValueTask<ValidationResult> Validate(UpdatePostRequest post);
    }
}