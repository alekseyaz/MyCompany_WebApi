using System;

namespace MyCompany.NameProject.Infrastructure.Persistence.Models
{
    public class WeatherHistoryEntity
    {
        public Guid Id { get; set; }

        public string Request { get; set; }

        public string Data { get; set; }

        public string ErrorDescription { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
