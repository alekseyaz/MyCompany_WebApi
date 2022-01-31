using System;

namespace MyCompany.NameProject.Infrastructure.Cache.Models
{
    public class CacheSettings
    {
        public const string SectionName = "CacheSettings";

        public int ValueLimitBytes { get; set; }

        public TimeSpan DefaultCacheTime { get; set; }
    }
}
