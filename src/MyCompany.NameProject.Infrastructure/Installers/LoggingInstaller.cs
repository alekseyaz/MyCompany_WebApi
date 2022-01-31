using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Infrastructure.Logging;

namespace MyCompany.NameProject.Infrastructure.Installers
{
    public class LoggingInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));
        }
    }
}
