namespace BlogManagement.Options
{
    public class JwtConfigOptions
    {
        public string Secret { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}