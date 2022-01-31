using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Application.Common.Interfaces.Geocoder;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.Weather;
using MyCompany.NameProject.Application.Common.Interfaces.Weather;
using MyCompany.NameProject.Domain.Entities.Geocoder;
using MyCompany.NameProject.Infrastructure.Weather.Mapping;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Infrastructure.Geocoder;
using MyCompany.NameProject.Infrastructure.Geocoder.Mapping;
using MyCompany.NameProject.Infrastructure.Geocoder.Models;
using MyCompany.NameProject.Infrastructure.Persistence.Mapping;
using MyCompany.NameProject.Infrastructure.Persistence.Models;

namespace MyCompany.NameProject.Infrastructure.Installers
{
    public class GeocoderInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGeocoderService, GeocoderService>();
            services.AddSingleton<IMapper<Coordinates, CoordinatesData>, CoordinatesDataMapper>();
        }
    }
}
