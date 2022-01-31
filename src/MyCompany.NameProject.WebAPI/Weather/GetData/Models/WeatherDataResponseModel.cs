using System.Text.Json;
using System.Text.Json.Serialization;

namespace MyCompany.NameProject.WebAPI.Weather.GetData.Models
{
    public class WeatherDataResponseModel
    {
        [JsonPropertyName("temp")]
        public int Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public int FeelsLike { get; set; }

        [JsonPropertyName("temp_water")]
        public int TempWater { get; set; }

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
