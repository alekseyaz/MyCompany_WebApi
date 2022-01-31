namespace MyCompany.NameProject.Domain.Entities.Weather
{
    public class WeatherData
    {
        public int Temp { get; set; }

        public int FeelsLike { get; set; }

        public int TempWater { get; set; }

        public string Condition { get; set; }

        public float WindSpeed { get; set; }

        public float WindGust { get; set; }

        public string WindDir { get; set; }

        public int PressureMm { get; set; }

        public int PrecType { get; set; }
    }
}
