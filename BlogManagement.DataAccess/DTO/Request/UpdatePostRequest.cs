using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogManagement.DataAccess.DTO.Request
{
    public class UpdatePostRequest
    {
        public UpdatePostRequest()
        {
            CategoryIds = new List<int>();
        }
        public string Title { get; set; }
        
        public string Body { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}