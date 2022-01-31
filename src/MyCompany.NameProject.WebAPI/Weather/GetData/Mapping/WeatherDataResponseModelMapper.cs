using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.WebAPI.Weather.GetData.Models;

namespace MyCompany.NameProject.WebAPI.Weather.GetData.Mapping
{
    public class WeatherDataResponseModelMapper : IMapper<WeatherData, WeatherDataResponseModel>
    {
        public WeatherDataResponseModel Map(WeatherData source)
        {
            if (source == null)
                return null;

            return new WeatherDataResponseModel
            {
                Temp = source.Temp,
                FeelsLike = source.FeelsLike,
                TempWater = source.FeelsLike,
                Condition = source.Condition,
                WindSpeed = source.WindSpeed,
                WindGust = source.WindGust,
                WindDir = source.WindDir,
                PressureMm = source.PressureMm,
                PrecType = source.PrecType
            };
        }
    }
}
