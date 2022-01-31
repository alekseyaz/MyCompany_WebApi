using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models;

namespace MyCompany.NameProject.Infrastructure.Weather.Mapping
{
    internal class WeatherDataMapper : IMapper<WeatherInformersResponse, WeatherData>
    {
        public WeatherData Map(WeatherInformersResponse source)
        {
            if (source?.Fact == null)
                return null;

            return new WeatherData
            {
                Temp = source.Fact.Temp,
                FeelsLike = source.Fact.FeelsLike,
                TempWater = source.Fact.FeelsLike,
                Condition = source.Fact.Condition,
                WindSpeed = source.Fact.WindSpeed,
                WindGust = source.Fact.WindGust,
                WindDir = source.Fact.WindDir,
                PressureMm = source.Fact.PressureMm,
                PrecType = source.Fact.PrecType
            };
        }
    }

}
