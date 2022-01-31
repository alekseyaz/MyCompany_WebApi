using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Application.Common.Exceptions;
using MyCompany.NameProject.Infrastructure.Installers;

namespace MyCompany.NameProject.WebAPI.Extensions
{
    /// <summary>
    /// Расширение для поиска и регистрации типов
    /// </summary>
    public static class ServiceInstallerExtension
    {
        /// <summary>
        /// Поиск типов, реализующих интерфейс IInstall и вызов метода Install для их регистрации в контейнере
        /// </summary>
        public static IServiceCollection InstallServicesByAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var installers = new List<IInstaller>();
            
            var apiInstallers = typeof(Program).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();
 
            var applicationInstallers = typeof(ValidationException).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();

            var infrastructureInstallers = typeof(WeatherInstaller).Assembly.ExportedTypes
                .Where(x => typeof(IInstaller).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IInstaller>().ToList();


            installers.AddRange(apiInstallers);

            installers.AddRange(applicationInstallers);

            installers.AddRange(infrastructureInstallers);

            installers.ForEach(x => x.Install(services, configuration));

            return services;
        }
    }
}
