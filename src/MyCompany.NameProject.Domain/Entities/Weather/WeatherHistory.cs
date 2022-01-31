using System;

namespace MyCompany.NameProject.Domain.Entities.Weather
{
    public class WeatherHistory
    {
        public Guid Id { get; set; }

        public string Request { get; set; }

        public string Data { get; set; }

        public string ErrorDescription { get; set; }

        public DateTime LastUpdateDate { get; set; }
    }
}
