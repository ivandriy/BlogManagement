using System.ComponentModel.DataAnnotations;

namespace BlogManagement.DataAccess.DTO.Request
{
    public class CreatePostRequest : UpdatePostRequest
    {
        [Required]
        public int BlogId { get; set; }
    }
}