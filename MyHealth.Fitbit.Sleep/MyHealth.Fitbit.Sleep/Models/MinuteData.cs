using System.Diagnostics.CodeAnalysis;

namespace MyHealth.Fitbit.Sleep.Models
{
    [ExcludeFromCodeCoverage]
    public class MinuteData
    {
        public string dateTime { get; set; }
        public string value { get; set; }
    }
}
