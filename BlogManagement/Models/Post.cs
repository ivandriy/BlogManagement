using System;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Topic { get; set; }
        public string Body { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public User CreatedBy { get; set; }
        public User UpdatedBy { get; set; }
    }
}