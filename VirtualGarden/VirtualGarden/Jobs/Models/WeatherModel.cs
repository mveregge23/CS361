using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualGarden.Jobs
{
    public class WeatherModel
    {
        public class CurrentDayData
        {
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class HourlyWeather
        {
            public int dt { get; set; }
            public Weather weather { get; set; }
        }

        public class Weather
        {
            public string main { get; set; }
        }

        public CurrentDayData current { get; set; }
        public List<HourlyWeather> hourly { get; set; }
    }
}