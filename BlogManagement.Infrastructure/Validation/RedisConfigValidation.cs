using BlogManagement.Infrastructure.Options;
using Microsoft.Extensions.Options;

namespace BlogManagement.Infrastructure.Validation
{
    public class RedisConfigValidation : IValidateOptions<RedisConfigurationOptions>
    {
        public ValidateOptionsResult Validate(string name, RedisConfigurationOptions options)
        {
            if (!options.IsEnabled) return ValidateOptionsResult.Success;
            
            if(options.AbsoluteExpirationMinutes < 1)
                return ValidateOptionsResult.Fail("Redis absolute expiration should not be less than 1 minute");
            if(options.SlidingExpirationMinutes < 1)
                return ValidateOptionsResult.Fail("Redis sliding expiration should not be less than 1 minute");
            return ValidateOptionsResult.Success;
        }
    }
}