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
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Body { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}