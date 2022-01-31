using MyCompany.NameProject.Domain.Interfaces;
using MyCompany.NameProject.Infrastructure.Persistence.Models;

namespace MyCompany.NameProject.Infrastructure.Persistence.Mapping
{
    public class WeatherHistoryEntityEnricher : IEnricher<WeatherHistoryEntity>
    {
        public void Enrich(WeatherHistoryEntity model, WeatherHistoryEntity data)
        {
            model.Id = data.Id;
            model.Request = data.Request;
            model.Data = data.Data;
            model.ErrorDescription = data.ErrorDescription;
            model.LastUpdateDate = data.LastUpdateDate;
        }
    }
}
