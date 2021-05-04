namespace MyHealth.Fitbit.Sleep.Models
{
    public class Summary
    {
        public Stages stages { get; set; }
        public int totalMinutesAsleep { get; set; }
        public int totalSleepRecords { get; set; }
        public int totalTimeInBed { get; set; }
    }
}
