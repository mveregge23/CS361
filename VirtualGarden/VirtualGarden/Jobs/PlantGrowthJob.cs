using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using VirtualGarden.Models;
using System.Threading.Tasks;
using System.Threading;
using System.Data.Entity.Spatial;
using VirtualGarden.WeatherData;
using Newtonsoft.Json;

namespace VirtualGarden.Jobs
{
    public class PlantGrowthJob
    {

        public class TimerState
        {
            public int Counter;
        }

        private readonly ApplicationDbContext _context;
        private readonly HttpClient client;
        
        public PlantGrowthJob()
        {
            _context = new ApplicationDbContext();
            client = new HttpClient();
        }


        public void UpdatePlantGrowth(object state)
        { 
            var timerState = state as TimerState;
            timerState.Counter = timerState.Counter + 1;
            System.Diagnostics.Debug.WriteLine("Updating plant growth for the ", timerState.Counter.ToString(), " time.");
            GrowPlants();
        }

        public async void GrowPlants()
        {
            TestWeatherData twd = new TestWeatherData();
            WeatherModel testWeather = twd.GetTestWeatherData();

            List<Location> locations = _context.Locations.ToList();
            Dictionary<int, WeatherModel> weathers = new Dictionary<int, WeatherModel>();
            string uri = "https://cs361-weather-service.herokuapp.com/history?";

            foreach (Location loc in locations)
            {

                string fullUri = uri + "latitude=" + loc.LatLon.Latitude + "&longitude=" + loc.LatLon.Longitude + "&time=" + (DateTimeOffset.Now.ToUnixTimeSeconds() - 86400);

                HttpResponseMessage response = await client.GetAsync(fullUri);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    string jsonContent = content.ReadAsStringAsync().Result;
                    
                    try
                    {
                        WeatherModel weather = JsonConvert.DeserializeObject<WeatherModel>(jsonContent);
                        weathers.Add(loc.Id, weather);
                    }
                    catch (JsonSerializationException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Problem deserializing json to weather model: {0}", ex.ToString()));
                    }
                }

            }

            List<Plant> plants = _context.Plants.ToList();

            foreach (Plant plant in plants)
            {
                int locationId = plant.Planter.Garden.LocationId;
                if (weathers.ContainsKey(locationId)) { 
                    System.Diagnostics.Debug.WriteLine(string.Format("Growing plant using live weather data"));
                    GrowPlant(plant, weathers[locationId]); 
                }
                else { 
                    System.Diagnostics.Debug.WriteLine(string.Format("Growing plant using test weather data"));
                    GrowPlant(plant, testWeather); 
                }
            }

        }

        public void GrowPlant(Plant plant, WeatherModel tw)
        {
            int currentDaySun = CalculateSun(tw);
            int currentDayWater = CalculateWater(tw);



            if (plant.Sun - (plant.PlantType.SunRequirement / 10) + currentDaySun > 100)
            {
                plant.Sun = 100;
            }
            else if (plant.Sun - (plant.PlantType.SunRequirement / 10) + currentDaySun < 0)
            {
                plant.Sun = 0;
            }
            else
            {
                plant.Sun = plant.Sun - (int)(plant.PlantType.SunRequirement / 10) + currentDaySun;
            }

            if (plant.Water - (plant.PlantType.WaterRequirement / 10) + currentDayWater > 100)
            {
                plant.Water = 100;
            }
            else if (plant.Water - (plant.PlantType.WaterRequirement / 10) + currentDayWater < 0)
            {
                plant.Water = 0;
            }
            else
            {
                plant.Water = plant.Water - (int)(plant.PlantType.WaterRequirement / 10) + currentDayWater;
            }

            _context.SaveChanges();

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

                if (plant.Growth - 5 < 0)
                {
                    plant.Growth = 0;
                }
                else
                {
                    plant.Growth = plant.Growth - 5;
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

            int water = 0;

            foreach (WeatherModel.HourlyWeather hw in weather.hourly)
            {
                if (hw.weather.main == "Rain" || hw.weather.main == "Thunderstorm" || hw.weather.main == "Drizzle")
                {
                    water = water + 1;
                }
            }

            return water;
        }
    }
}