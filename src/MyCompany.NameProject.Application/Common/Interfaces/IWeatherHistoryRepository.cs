using System;
using System.Threading;
using System.Threading.Tasks;
using MyCompany.NameProject.Domain.Entities.Weather;

namespace MyCompany.NameProject.Application.Common.Interfaces
{
    public interface IWeatherHistoryRepository
    {
        Task<WeatherHistory> GetWeatherHistoryAsync(
            Guid id, CancellationToken cancellationToken);

        Task<WeatherHistory> CreateWeatherHistoryAsync(
            WeatherHistory weatherHistory, CancellationToken cancellationToken);

        Task UpdateWeatherHistoryAsync(
            WeatherHistory weatherHistory, CancellationToken cancellationToken);

        Task<bool> DeleteWeatherHistoryAsync(
            Guid id, CancellationToken cancellationToken);
    }
}
