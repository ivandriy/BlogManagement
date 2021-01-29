using System.Collections.Generic;

namespace BlogManagement.DataAccess.DTO.Response
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public bool Success { get; set; }

        public List<string> Errors { get; set; }
    }
}