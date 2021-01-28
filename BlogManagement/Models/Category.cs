using System.Collections.Generic;

namespace BlogManagement.Models
{
    public class Category
    {
        public Category()
        {
            CategoryPosts = new List<Post>();
        }
        public int CategoryId { get; set; }

        public string Name { get; set; }
        
        // Many-to-many relationship to Post
        public List<Post> CategoryPosts { get; set; }
    }
}