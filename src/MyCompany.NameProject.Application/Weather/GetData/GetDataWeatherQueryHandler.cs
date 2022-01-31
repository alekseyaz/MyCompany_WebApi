using MediatR;
using MyCompany.NameProject.Application.Common.Exceptions;
using MyCompany.NameProject.Application.Common.Interfaces;
using MyCompany.NameProject.Application.Common.Interfaces.Weather;
using MyCompany.NameProject.Domain.Entities.Weather;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.NameProject.Application.Weather.GetData
{
    internal class GetDataWeatherQueryHandler : IRequestHandler<GetDataWeatherQuery, WeatherData>
    {
        private readonly IWeatherService _weatherService;
        private readonly IWeatherHistoryRepository _weatherHistoryRepository;

        public GetDataWeatherQueryHandler(
            IWeatherService weatherService,
            IWeatherHistoryRepository weatherHistoryRepository)
        {
            _weatherService = weatherService;
            _weatherHistoryRepository = weatherHistoryRepository;
        }

        public async Task<WeatherData> Handle(GetDataWeatherQuery request, CancellationToken cancellationToken)
        {
            var data = await _weatherService.GetWeatherDataAsync(request.City, cancellationToken);

            if (data == null)
                throw new NotFoundException($"Weather not found");

            var weatherHistory = await _weatherHistoryRepository.CreateWeatherHistoryAsync(new WeatherHistory
            {
                Data = JsonSerializer.Serialize(data),
                Request = JsonSerializer.Serialize(request)
            }, cancellationToken);

            return data;
        }
    }
}
