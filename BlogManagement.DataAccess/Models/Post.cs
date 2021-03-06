using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlogManagement.DataAccess.Models
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
        
        public string UserName { get; set; }
        
        // Blog relation (1-to-many)
        public int BlogId { get; set; }

        //Many-to-many relationship to Category
        [JsonIgnore]
        public List<Category> Categories { get; set; }

        [NotMapped]
        [JsonInclude]
        public List<string> CategoriesNames => Categories.Select(c => c.Name).ToList();
    }
}