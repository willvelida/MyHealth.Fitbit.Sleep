using System.Diagnostics.CodeAnalysis;

namespace MyHealth.Fitbit.Sleep.Models
{
    [ExcludeFromCodeCoverage]
    public class Summary
    {
        public Stages stages { get; set; }
        public int totalMinutesAsleep { get; set; }
        public int totalSleepRecords { get; set; }
        public int totalTimeInBed { get; set; }
    }
}
