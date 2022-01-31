using System.Text.Json.Serialization;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models
{
    public class WeatherInformersResponse
    {
        [JsonPropertyName("now")]
        public int Now { get; set; }

        [JsonPropertyName("now_dt")]
        public string NowDataTime { get; set; }

        [JsonPropertyName("fact")]
        public WeatherInformersFactResponse Fact { get; set; }
    }
}
