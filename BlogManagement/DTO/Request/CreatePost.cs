using System.ComponentModel.DataAnnotations;

namespace BlogManagement.DTO.Request
{
    public class CreatePost : UpdatePost
    {
        [Required] public int BlogId { get; set; }

        [Required] public int UserId { get; set; }
    }
}