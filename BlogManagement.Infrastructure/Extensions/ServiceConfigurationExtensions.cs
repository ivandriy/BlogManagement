using System;
using System.Text;
using BlogManagement.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BlogManagement.Infrastructure.Extensions
{
    public static class ServiceConfigurationExtensions
    {
        public static void AddAuthenticationWithJwtBearerToken(this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("JwtConfig").Get<JwtConfigOptions>().ValidateJwtConfig();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = true;
                    opt.SaveToken = true;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        RequireExpirationTime = false
                    };
                });
        }

        public static string GetRedisConfiguration(this IConfiguration configuration) => configuration.GetSection("Redis").Get<RedisConfigurationOptions>().ValidateRedisConfig().Connection;

        public static string GetBlogDbConnectionString(this IConfiguration configuration)
        {
            var configSection = configuration.GetSection("BlogDbConfig");
            var host = configSection.GetValue<string>("Host");
            var port = configSection.GetValue<int>("Port");
            var database = configSection.GetValue<string>("Database");
            var user = configSection.GetValue<string>("Username");
            var password = configSection.GetValue<string>("Password");

            return $"Host={host};Port={port};Database={database};Username={user};Password={password}";
        }
        
        private static JwtConfigOptions ValidateJwtConfig(this JwtConfigOptions config)
        {
            if (string.IsNullOrWhiteSpace(config.Secret) || config.Secret.Length < 20)
                throw new ArgumentException(
                    "JwtConfig secret string should not be empty or contain less than 20 symbols");
            return config;
        }

        private static RedisConfigurationOptions ValidateRedisConfig(this RedisConfigurationOptions config)
        {
            if (config.IsEnabled && string.IsNullOrWhiteSpace(config.Connection))
                throw new ArgumentException("Redis connection should not be empty");
            return config;
        }
        
    }
}