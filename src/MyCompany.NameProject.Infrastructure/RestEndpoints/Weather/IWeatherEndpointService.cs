using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Weather
{
    public interface IWeatherEndpointService
    {
        Task<WeatherInformersResponse> InformersAsync(
            WeatherInformersRequest request, CancellationToken cancellationToken);
    }
}
