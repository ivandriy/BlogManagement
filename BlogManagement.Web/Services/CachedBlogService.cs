using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlogManagement.DataAccess.DTO.Response;
using BlogManagement.Infrastructure.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlogManagement.Services
{
    public class CachedBlogService : IBlogService
    {
        private readonly IBlogService _repository;
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<CachedBlogService> _logger;
        private readonly RedisConfigurationOptions _redisOptions;

        public CachedBlogService( 
            IBlogService repository,
            IDistributedCache distributedCache,
            IOptionsSnapshot<RedisConfigurationOptions> redisOptions,
            ILogger<CachedBlogService> logger)
        {
            _repository = repository;
            _distributedCache = distributedCache;
            _logger = logger;
            _redisOptions = redisOptions.Value;
        }
        public async Task<IEnumerable<PostViewModel>> GetBlogPosts(int blogId)
        {
            
            List<PostViewModel> postsViewModels;
            if (!_redisOptions.IsEnabled)
            {
                _logger.LogInformation($"Loading posts for blogId {blogId} from database", blogId);
                postsViewModels = (await _repository.GetBlogPosts(blogId)).ToList();
            }
            else
            {
                _logger.LogInformation($"Trying to fetch posts for blogId {blogId} from cache", blogId);
                string postsSerialized;
                var cacheKey = $"blogPosts:{blogId}";
                var encodedPosts = await _distributedCache.GetAsync(cacheKey);
                if (encodedPosts != null)
                {
                    postsSerialized = Encoding.UTF8.GetString(encodedPosts);
                    postsViewModels = JsonSerializer.Deserialize<List<PostViewModel>>(postsSerialized);
                    _logger.LogInformation($"Loaded {postsViewModels?.Count} post for blogId {blogId} from cache", postsViewModels);
                }
                else
                {
                    _logger.LogInformation($"Loading posts for blogId {blogId} from database", blogId);
                    postsViewModels = (await _repository.GetBlogPosts(blogId)).ToList();
                    postsSerialized = JsonSerializer.Serialize(postsViewModels);
                    encodedPosts = Encoding.UTF8.GetBytes(postsSerialized);
                    var cacheOptions = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(_redisOptions.SlidingExpirationMinutes))
                        .SetAbsoluteExpiration(TimeSpan.FromMinutes(_redisOptions.AbsoluteExpirationMinutes));
                    await _distributedCache.SetAsync(cacheKey, encodedPosts, cacheOptions);
                    _logger.LogInformation($"Added {postsViewModels.Count} post for blogId {blogId} into cache", postsViewModels);
                }
            }

            return postsViewModels;
        }
    }
}