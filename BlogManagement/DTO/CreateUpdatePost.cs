namespace BlogManagement.DTO
{
    public class CreateUpdatePost
    {
        public string Title { get; set; }
        
        public string Body { get; set; }
        
        public int BlogId { get; set; }
    }
}