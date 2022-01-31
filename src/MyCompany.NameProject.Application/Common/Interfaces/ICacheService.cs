using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MyCompany.NameProject.Application.Common.Interfaces
{
    public interface ICacheService
    {
        void SetString(string key, string value, TimeSpan? cacheTime = null);

        string GetString(string key);

        void Delete(string key);

        Task SetStringAsync(string key, string value, TimeSpan? cacheTime = null);

        Task<string> GetStringAsync(string key);

        Task DeleteAsync(string key);

        ILogger<ICacheService> GetLogger();
    }
}
