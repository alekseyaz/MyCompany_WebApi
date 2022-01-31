using System;

namespace MyCompany.NameProject.Infrastructure.RestEndpoints.Weather
{
    public class WeatherEndpointSettings
    {
        public const string SectionName = "WeatherEndpointSettings";

        /// <summary>
        /// Service Uri
        /// </summary>
        public string BaseApiUrl { get; set; }

        /// <summary>
        /// Service auth token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Cache time
        /// </summary>
        public TimeSpan? CacheInterval { get; set; }

        /// <summary>
        /// Service call timeout
        /// </summary>
        public TimeSpan Timeout { get; set; }
    }
}
