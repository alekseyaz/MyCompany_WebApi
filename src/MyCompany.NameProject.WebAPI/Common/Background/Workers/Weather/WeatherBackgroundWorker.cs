using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyCompany.NameProject.Application.Common.Interfaces.Geocoder;
using MyCompany.NameProject.Application.Weather.GetData;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.NameProject.WebAPI.Common.Background.Workers.Weather
{
    public class WeatherBackgroundWorker : BackgroundWorker
    {
        private readonly WeatherBackgroundWorkerSettings _settings;

        public WeatherBackgroundWorker(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<WeatherBackgroundWorkerSettings> settings,
            ILogger<BackgroundWorker> logger)
            : base(serviceScopeFactory, settings.Value, logger)
        {
            _settings = settings.Value;
        }

        protected override async Task ExecuteOnceAsync(IServiceProvider serviceProvider,
            CancellationToken cancellationToken)
        {
            var geocoderService = serviceProvider.GetService<IGeocoderService>();
            var mediator = serviceProvider.GetService<IMediator>();

            await CheckWeatherAsync(geocoderService, mediator, cancellationToken);
        }

        private async Task CheckWeatherAsync(
            IGeocoderService geocoderService,
            ISender mediator,
            CancellationToken cancellationToken)
        {
            var cities = geocoderService.GetAllCities();

            foreach (var city in cities)
            {
                try
                {
                    await mediator.Send(new GetDataWeatherQuery { City = city }, cancellationToken);
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Failed to complete weather query");
                }
            }
        }
    }
}
