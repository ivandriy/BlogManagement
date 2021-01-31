namespace BlogManagement.Infrastructure.Options
{
    public class RedisOptions
    {
        public string Connection { get; set; }

        public int SlidingExpirationMinutes { get; set; }
        
        public int AbsoluteExpirationMinutes { get; set; }
        
        public bool IsEnabled { get; set; }
    }
}