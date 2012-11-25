using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace TLogger
{
    public class TemperatureItem
    {
        public TemperatureItem()
        {
        }

        public TemperatureItem(Timestamped<double> tsd)
        {
            this.Temperature = tsd.Value;
            this.Timestamp = tsd.Timestamp.DateTime;
        }

        public static TemperatureItem FromTDS(Timestamped<double> tsd)
        {
            return new TemperatureItem(tsd);
        }

        public DateTime Timestamp { get; set; }
        public double Temperature { get; set; }
    }
}
