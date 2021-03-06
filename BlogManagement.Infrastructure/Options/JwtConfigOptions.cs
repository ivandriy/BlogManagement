using System.ComponentModel.DataAnnotations;

namespace BlogManagement.Infrastructure.Options
{
    public class JwtConfigOptions
    {
        public string Secret { get; set; }
        
        public int AccessTokenExpiration { get; set; }
        
        public int RefreshTokenExpiration { get; set; }
    }
}