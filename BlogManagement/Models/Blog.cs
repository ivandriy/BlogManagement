using System.Collections.Generic;

namespace BlogManagement.Models
{
    public class Blog
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Post> BlogPosts { get; set; }
    }
}