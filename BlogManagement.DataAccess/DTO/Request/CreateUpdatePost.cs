using System.Collections.Generic;

namespace BlogManagement.DataAccess.DTO.Request
{
    public class CreateUpdatePost
    {
        public CreateUpdatePost()
        {
            CategoryIds = new List<int>();
        }
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public int BlogId { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}