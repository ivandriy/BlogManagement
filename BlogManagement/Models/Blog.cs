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
        
        // 1-to-1 to Theme
        public Theme Theme { get; set; }

        // 1-to-many relation to Posts
        public List<Post> BlogPosts { get; set; }
        
        //1-1 relation to Author
        public Author BlogAuthor { get; set; }
    }
}