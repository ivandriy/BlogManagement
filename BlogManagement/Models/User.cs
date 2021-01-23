using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BlogManagement.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        
        public List<Post> Posts { get; set; }
    }
}