using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MyHealth.Fitbit.Sleep.Models
{
    [ExcludeFromCodeCoverage]
    public class SleepResponseObject
    {
        public List<Sleep> sleep { get; set; }
        public Summary summary { get; set; }
    }
}
