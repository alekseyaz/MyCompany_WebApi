using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.Weather;
using MyCompany.NameProject.Application.Common.Interfaces.Weather;
using MyCompany.NameProject.Infrastructure.Weather.Mapping;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Infrastructure.Persistence.Mapping;
using MyCompany.NameProject.Infrastructure.Persistence.Models;

namespace MyCompany.NameProject.Infrastructure.Installers
{
    public class WeatherInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddSingleton<IMapper<WeatherInformersResponse, WeatherData>, WeatherDataMapper>();

            services.AddScoped<IMapper<WeatherHistoryEntity, WeatherHistory>, WeatherHistoryEntityMapper>();
            services.AddScoped<IMapper<WeatherHistory, WeatherHistoryEntity>, WeatherHistoryEntityMapper>();
            services.AddScoped<IEnricher<WeatherHistoryEntity>, WeatherHistoryEntityEnricher>();

            services.AddScoped<IWeatherHistoryRepository, WeatherHistoryRepository>();
        }
    }
}
