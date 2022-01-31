using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MyCompany.NameProject.WebAPI.Common.Background
{
    public abstract class BackgroundWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly BackgroundWorkerSettings _settings;

        protected readonly ILogger<BackgroundWorker> Logger;

        protected BackgroundWorker(
            IServiceScopeFactory serviceScopeFactory,
            BackgroundWorkerSettings settings,
            ILogger<BackgroundWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _settings = settings;
            Logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(_settings.TimerOffset, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    try
                    {
                        await ExecuteOnceAsync(scope.ServiceProvider, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Unhandled exception while executing background task");
                    }
                }

                await Task.Delay(_settings.TimerPeriod, stoppingToken);
            }
        }

        protected abstract Task ExecuteOnceAsync(IServiceProvider serviceProvider, CancellationToken cancellationToken);
    }
}
