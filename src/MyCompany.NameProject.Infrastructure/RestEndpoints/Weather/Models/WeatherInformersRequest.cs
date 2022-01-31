using System.Text.Json.Serialization;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models
{
    public class WeatherInformersRequest
    {
        [JsonPropertyName("lat")]
        public string Lat { get; set; }

        [JsonPropertyName("lon")]
        public string Lon { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; }
    }
}
