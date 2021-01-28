using System.Collections.Generic;

namespace BlogManagement.DTO.Response
{
    public class AuthenticationResult
    {
        public string Token { get; set; }

        public bool Success { get; set; }

        public List<string> Errors { get; set; }
    }
}