using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Infrastructure.Cache.Models;

namespace MyCompany.NameProject.Infrastructure.Cache
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;
        private readonly CacheSettings _cacheSettings;
        private readonly IDistributedCache _distributedCache;

        public CacheService(ILogger<CacheService> logger, IOptions<CacheSettings> commonSettings, IDistributedCache distributedCache)
        {
            _logger = logger;
            _cacheSettings = commonSettings.Value;
            _distributedCache = distributedCache;
        }

        public void SetString(string key, string value, TimeSpan? cacheTime = null)
        {
            if (cacheTime?.Ticks <= 0)
                return;

            var valueBytesSize = Encoding.UTF8.GetByteCount(value);
            if (valueBytesSize > _cacheSettings.ValueLimitBytes)
            {
                _logger.LogError($"Cache: store error (trying to store {valueBytesSize}bytes instead of limit {_cacheSettings.ValueLimitBytes}bytes), key - {key}");
                return;
            }

            try
            {
                _distributedCache.SetString(key, value, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = cacheTime ?? _cacheSettings.DefaultCacheTime
                });
                _logger.LogTrace($"Cache: store info. Key: {key}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache: store exception, key - {key}");
            }
        }

        public string GetString(string key)
        {
            try
            {
                return _distributedCache.GetString(key);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache: get data exception, key - {key}");
                return null;
            }
        }

        public void Delete(string key)
        {
            try
            {
                _distributedCache.Remove(key);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache: deleting record exception, key - {key}");
            }
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? cacheTime = null)
        {
            if (cacheTime?.Ticks <= 0)
                return;

            var valueBytesSize = Encoding.UTF8.GetByteCount(value);
            if (valueBytesSize > _cacheSettings.ValueLimitBytes)
            {
                _logger.LogError($"Cache: store error (trying to store {valueBytesSize}bytes instead of limit {_cacheSettings.ValueLimitBytes}bytes), key - {key}");
                return;
            }

            try
            {
                await _distributedCache.SetStringAsync(key, value, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = cacheTime ?? _cacheSettings.DefaultCacheTime
                });
                _logger.LogTrace($"Cache: store info. Key: {key}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache: store exception, key - {key}");
            }
        }

        public async Task<string> GetStringAsync(string key)
        {
            try
            {
                return await _distributedCache.GetStringAsync(key);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache: get data exception, key - {key}");
                return string.Empty;
            }
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cache: deleting record exception, key - {key}");
            }
        }

        public ILogger<ICacheService> GetLogger()
        {
            return _logger;
        }
    }
}
