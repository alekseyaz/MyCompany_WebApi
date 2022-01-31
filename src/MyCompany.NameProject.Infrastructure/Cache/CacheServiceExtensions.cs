using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MyCompany.NameProject.Application.Common.Interfaces;

namespace MyCompany.NameProject.Infrastructure.Cache
{
    public static class CacheServiceExtensions
    {
        public static void Set<T>(this ICacheService service, string key, T val, TimeSpan? cacheTime = null)
        {
            service.SetString(key, JsonSerializer.Serialize(val), cacheTime);
        }

        public static T Get<T>(this ICacheService service, string key)
        {
            try
            {
                var val = service.GetString(key);
                return val == null ? default : JsonSerializer.Deserialize<T>(val);
            }
            catch (Exception e)
            {
                service.GetLogger().LogError(e, $"Error deserializing object of type '{typeof(T).Name}' for key '{key}'");

                return default;
            }
        }

        public static T GetCached<T>(this ICacheService service, string key, Func<T> valueFunc, TimeSpan? expiry = null)
        {
            try
            {
                if (expiry?.Ticks <= 0)
                    return valueFunc();

                service.GetLogger().LogTrace($"Get cached data by key {key}");

                var data = Get<T>(service, key);

                if (!data?.Equals(default(T)) ?? false)
                    return data;

                service.GetLogger().LogTrace($"Key {key} not exist. Execute function");

                var item = valueFunc();
                Set(service, key, item, expiry);

                return item;
            }
            catch (Exception ex)
            {
                service.GetLogger().LogError(ex, ex.Message);
                throw;
            }
        }

        public static async Task SetAsync<T>(this ICacheService service, string key, T val, TimeSpan? cacheTime = null)
        {
            await service.SetStringAsync(key, JsonSerializer.Serialize(val), cacheTime);
        }

        public static async Task<T> GetAsync<T>(this ICacheService service, string key)
        {
            try
            {
                var val = await service.GetStringAsync(key);
                return val == null ? default : JsonSerializer.Deserialize<T>(val);
            }
            catch (Exception e)
            {
                service.GetLogger().LogError(e, $"Error deserializing object of type '{typeof(T).Name}' for key '{key}'");

                return default;
            }
        }

        public static async Task<T> GetCachedAsync<T>(this ICacheService service, string key, Func<Task<T>> valueFunc, TimeSpan? expiry = null)
        {
            try
            {
                if (expiry?.Ticks <= 0)
                    return await valueFunc();

                service.GetLogger().LogTrace($"Get cached data by key {key}");

                var data = await GetAsync<T>(service, key);

                if (!data?.Equals(default(T)) ?? false)
                    return data;

                service.GetLogger().LogTrace($"Key {key} not exist. Execute function");

                var item = await valueFunc();
                await SetAsync(service, key, item, expiry);

                return item;
            }
            catch (Exception ex)
            {
                service.GetLogger().LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
