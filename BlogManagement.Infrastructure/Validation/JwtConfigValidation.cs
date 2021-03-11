using BlogManagement.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace BlogManagement.Infrastructure.Validation
{
    public class JwtConfigValidation : IValidateOptions<JwtConfigOptions>
    {
        public ValidateOptionsResult Validate(string name, JwtConfigOptions options)
        {
            if(options.AccessTokenExpiration > 60)
                return ValidateOptionsResult.Fail("Jwt access token expiration value should not be greater than 60");
            if(options.RefreshTokenExpiration > 30)
                return ValidateOptionsResult.Fail("Jwt refresh token expiration value should not be greater than 30");
            return ValidateOptionsResult.Success;
        }
    }
}