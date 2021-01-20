using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<Post> BlogPosts { get; set; }
    }
}