using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtualGarden.Jobs;

namespace VirtualGarden.WeatherData
{
    public class TestWeatherData
    {
        public WeatherModel GetTestWeatherData()
        {

            List<WeatherModel.HourlyWeather> hourlyWeather = new List<WeatherModel.HourlyWeather>();

            for (int i = 1627790400; i < 1627876800; i = i + 3600)
            {
                hourlyWeather.Add(new WeatherModel.HourlyWeather { dt = i, weather = new WeatherModel.Weather{ main = "Clear" } } );
            }

            WeatherModel test = new WeatherModel
            {
                current = new WeatherModel.CurrentDayData
                {
                    sunrise = 1627814977,
                    sunset = 1627864678
                },

                hourly = hourlyWeather
            };

            return test;
        }
    }
}