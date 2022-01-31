using MyCompany.NameProject.Application.Common.Interfaces.Geocoder;
using MyCompany.NameProject.Application.Common.Interfaces.Weather;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models;
using System.Threading;
using System.Threading.Tasks;

namespace MyCompany.NameProject.Infrastructure.Weather
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherEndpointService _weatherEndpointService;
        private readonly IGeocoderService _geocoderService;
        private readonly IMapper<WeatherInformersResponse, WeatherData> _dataMapper;

        public WeatherService(
            IWeatherEndpointService weatherEndpointService,
            IGeocoderService geocoderService,
            IMapper<WeatherInformersResponse, WeatherData> dataMapper)
        {
            _weatherEndpointService = weatherEndpointService;
            _geocoderService = geocoderService;
            _dataMapper = dataMapper;
        }

        public async Task<WeatherData> GetWeatherDataAsync(string city, CancellationToken cancellationToken)
        {
            var coordinates = _geocoderService.GetCoordinates(city);

            var request = new WeatherInformersRequest
            {
                Lat = coordinates.Lat,
                Lon = coordinates.Lon
            };

            var response = await _weatherEndpointService.InformersAsync(request, cancellationToken);

            return _dataMapper.Map(response);
        }
    }
}
