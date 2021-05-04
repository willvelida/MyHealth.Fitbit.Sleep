using System.Collections.Generic;

namespace MyHealth.Fitbit.Sleep.Models
{
    public class SleepResponseObject
    {
        public List<Sleep> sleep { get; set; }
        public Summary summary { get; set; }
    }
}
