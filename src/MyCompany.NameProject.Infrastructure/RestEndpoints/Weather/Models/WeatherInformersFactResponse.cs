using System.Text.Json.Serialization;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Weather.Models
{
    public class WeatherInformersFactResponse
    {
        [JsonPropertyName("temp")]
        public int Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public int FeelsLike { get; set; }

        [JsonPropertyName("condition")]
        public string Condition { get; set; }

        [JsonPropertyName("wind_speed")]
        public float WindSpeed { get; set; }

        [JsonPropertyName("wind_gust")]
        public float WindGust { get; set; }

        [JsonPropertyName("wind_dir")]
        public string WindDir { get; set; }

        [JsonPropertyName("pressure_mm")]
        public int PressureMm { get; set; }

        [JsonPropertyName("prec_type")]
        public int PrecType { get; set; }
    }
}
