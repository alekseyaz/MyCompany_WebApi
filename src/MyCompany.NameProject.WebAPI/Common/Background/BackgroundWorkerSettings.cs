using System;

namespace MyCompany.NameProject.WebAPI.Common.Background
{
    public class BackgroundWorkerSettings
    {
        public TimeSpan TimerOffset { get; set; }

        public TimeSpan TimerPeriod { get; set; }
    }
}
