using System;
using System.Text.Json.Serialization;

namespace BlogManagement.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        
        // Blog relation
        public int BlogId { get; set; }
        [JsonIgnore]
        public Blog Blog { get; set; }
        
        public string UserName { get; set; }
    }
}