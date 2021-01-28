using System.Collections.Generic;

namespace BlogManagement.Models
{
    public class Blog
    {
        public Blog()
        {
            BlogPosts = new List<Post>();
        }
        public int BlogId { get; set; }
        public string Name { get; set; }

        public List<Post> BlogPosts { get; set; }
    }
}