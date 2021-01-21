using System;

namespace BlogManagement.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        
        public User CreatedBy { get; set; }
        public User UpdatedBy { get; set; }
    }
}