using MyCompany.NameProject.Domain.Entities.Weather;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.NameProject.Application.Common.Interfaces.Weather
{
    public interface IWeatherService
    {
        Task<WeatherData> GetWeatherDataAsync(
            string city, CancellationToken cancellationToken);
    }
}
