using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Infrastructure.Cache;
using MyCompany.NameProject.Infrastructure.Cache.Models;

namespace MyCompany.NameProject.Infrastructure.Installers
{
    public class CacheInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();

            services.AddOptions().Configure<CacheSettings>(configuration.GetSection(CacheSettings.SectionName));

            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}
