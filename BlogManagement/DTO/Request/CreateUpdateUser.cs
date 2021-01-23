using System.ComponentModel.DataAnnotations;

namespace BlogManagement.DTO.Request
{
    public class CreateUpdateUser
    {
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}