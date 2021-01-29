namespace BlogManagement.DataAccess.DTO.Request
{
    public class CreateUpdatePost
    {
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public int BlogId { get; set; }

        public int[] CategoryIds { get; set; }
    }
}