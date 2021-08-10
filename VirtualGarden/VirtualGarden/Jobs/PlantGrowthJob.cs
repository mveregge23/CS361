using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtualGarden.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Entity.Spatial;
using VirtualGarden.WeatherData;

namespace VirtualGarden.Jobs
{
    public class PlantGrowthJob
    {

        public class TimerState
        {
            public int Counter;
        }

        private readonly ApplicationDbContext _context;
        
        public PlantGrowthJob()
        {
            _context = new ApplicationDbContext();
        }


        public void UpdatePlantGrowth(object state)
        { 
            var timerState = state as TimerState;
            timerState.Counter = timerState.Counter + 1;
            System.Diagnostics.Debug.WriteLine("Updating plant growth for the ", timerState.Counter.ToString(), " time.");
            GrowPlants();
        }

        public void GrowPlants()
        {
            TestWeatherData twd = new TestWeatherData();
            WeatherModel testWeather = twd.GetTestWeatherData();

            List<Plant> plants = _context.Plants.ToList();

            foreach(Plant plant in plants)
            {
                GrowPlant(plant, testWeather);
            }
        }

        public void GrowPlant(Plant plant, WeatherModel tw)
        {
            int currentDaySun = CalculateSun(tw);
            int currentDayWater = CalculateWater(tw);



            if (plant.Sun - 5 + currentDaySun > 100)
            {
                plant.Sun = 100;
            }
            else if (plant.Sun - 5 + currentDaySun < 0)
            {
                plant.Sun = 0;
            }
            else
            {
                plant.Sun = plant.Sun - 5 + currentDaySun;
            }

            if (plant.Water - 5 + currentDayWater > 100)
            {
                plant.Water = 100;
            }
            else if (plant.Water - 5 + currentDayWater < 0)
            {
                plant.Water = 0;
            }
            else
            {
                plant.Water = plant.Water - 5 + currentDayWater;
            }

            if (plant.Sun > plant.PlantType.SunRequirement && plant.Water > plant.PlantType.WaterRequirement)
            {
                
                if (plant.Growth + 10 >= 100)
                {
                    plant.Growth = 100;
                }
                else
                {
                    plant.Growth = plant.Growth + 10;
                }
            }

            else
            {

                if (plant.Growth - 10 < 0)
                {
                    plant.Growth = 0;
                }
                else
                {
                    plant.Growth = plant.Growth - 10;
                }

            }

            _context.SaveChanges();
        }

        public int CalculateSun(WeatherModel weather)
        {
            
            int sunrise = weather.current.sunrise;
            int sunset = weather.current.sunset;

            int sun = 0;
 

            foreach(WeatherModel.HourlyWeather hw in weather.hourly)
            {
                if (hw.weather.main == "Clear" && hw.dt > sunrise && hw.dt < sunset)
                {
                    sun = sun + 1;
                }
            }

            return sun ;
        }

        public int CalculateWater(WeatherModel weather)
        {

            int sunrise = weather.current.sunrise;
            int sunset = weather.current.sunset;

            int water = 0;

            foreach (WeatherModel.HourlyWeather hw in weather.hourly)
            {
                if (hw.weather.main == "Rain")
                {
                    water = water + 1;
                }
            }

            return water;
        }
    }
}