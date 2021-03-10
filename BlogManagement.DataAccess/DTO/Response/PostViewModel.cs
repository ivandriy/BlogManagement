using System;
using System.Collections.Generic;

namespace BlogManagement.DataAccess.DTO.Response
{
    public class PostViewModel
    {
        public int PostId { get; set; }
        
        public string Title { get; set; }
        
        public string Content { get; set; }
        
        public DateTimeOffset? CreatedOn { get; set; }
        
        public DateTimeOffset? UpdatedOn { get; set; }
        
        public string UserName { get; set; }
        
        public IEnumerable<string> CategoryNames { get; set; }
    }
}