using MediatR;
using MyCompany.NameProject.Domain.Entities.Weather;

namespace MyCompany.NameProject.Application.Weather.GetData
{
    public class GetDataWeatherQuery : IRequest<WeatherData>
    {
        public string City { get; set; }
    }
}
