using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlogManagement.DataAccess.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Category
    {
        public Category()
        {
            CategoryPosts = new List<Post>();
        }
        public int CategoryId { get; set; }

        public string Name { get; set; }
        
        // Many-to-many relationship to Post
        [JsonIgnore]
        public List<Post> CategoryPosts { get; set; }

        [NotMapped] 
        [JsonInclude] 
        public List<int> CategoryPostIds => CategoryPosts.Select(p => p.PostId).ToList();
    }
}