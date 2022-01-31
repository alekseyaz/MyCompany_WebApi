using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCompany.NameProject.Application.Common;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.WebAPI.Weather.GetData.Mapping;
using MyCompany.NameProject.WebAPI.Weather.GetData.Models;

namespace MyCompany.NameProject.WebAPI.Common.Installers
{
    public class WeatherInstaller : IInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<
                IMapper<WeatherData, WeatherDataResponseModel>,
                WeatherDataResponseModelMapper>();
        }
    }
}
