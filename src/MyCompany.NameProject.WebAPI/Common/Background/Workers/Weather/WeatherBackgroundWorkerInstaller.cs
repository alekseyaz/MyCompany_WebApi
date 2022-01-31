using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;

namespace MyCompany.NameProject.WebAPI.Common.Background.Workers.Weather
{
    public class WeatherBackgroundWorkerInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<WeatherBackgroundWorker>();

            services.AddOptions().Configure<WeatherBackgroundWorkerSettings>(
                configuration.GetSection(WeatherBackgroundWorkerSettings.SectionName));
        }
    }
}
