using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyCompany.NameProject.Application.Common;
using System.Net.Http;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Installers
{
    public class WeatherEndpointServiceInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions().Configure<WeatherEndpointSettings>(
                configuration.GetSection(WeatherEndpointSettings.SectionName));

            services.AddHttpClient<IWeatherEndpointService, WeatherEndpointService>((provider, client) =>
            {
                var settings = provider.GetService<IOptions<WeatherEndpointSettings>>().Value;
                client.Timeout = settings.Timeout;
            })
                .ConfigurePrimaryHttpMessageHandler(provider => new HttpClientHandler { UseProxy = false });
        }
    }
}
