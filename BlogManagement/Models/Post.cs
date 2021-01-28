using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlogManagement.Models
{
    public class Post
    {
        public Post()
        {
            Categories = new List<Category>();
        }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }
        public DateTimeOffset? UpdatedOn { get; set; }
        
        // Blog relation (1-to-many)
        [JsonIgnore]
        public Blog Blog { get; set; }
        
        public string UserName { get; set; }
        
        //Many-to-many relationship to Category
        public List<Category> Categories { get; set; }
    }
}