using System;
using MyCompany.NameProject.Domain.Entities.Weather;
using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.Persistence.Models;

namespace MyCompany.NameProject.Infrastructure.Persistence.Mapping
{
    public class WeatherHistoryEntityMapper :
        IMapper<WeatherHistoryEntity, WeatherHistory>,
        IMapper<WeatherHistory, WeatherHistoryEntity>
    {
        public WeatherHistory Map(WeatherHistoryEntity source)
        {
            if (source == null)
                return null;

            return new WeatherHistory
            {
                Id = source.Id,
                Request = source.Request,
                Data = source.Data,
                ErrorDescription = source.ErrorDescription,
                LastUpdateDate = source.LastUpdateDate
            };
        }

        public WeatherHistoryEntity Map(WeatherHistory source)
        {
            if (source == null)
                return null;

            return new WeatherHistoryEntity
            {
                Id = source.Id,
                Request = source.Request,
                Data = source.Data,
                ErrorDescription = source.ErrorDescription,
                LastUpdateDate = DateTime.UtcNow
            };
        }
    }
}
