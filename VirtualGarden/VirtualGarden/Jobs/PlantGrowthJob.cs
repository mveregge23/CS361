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

        // Update plant growth for all plants -- called by timer in Global.asax
        public void UpdatePlantGrowth(object state)
        { 
            var timerState = state as TimerState;
            timerState.Counter = timerState.Counter + 1;
            GrowPlants();
        }

        // Find and grow all plants
        public async void GrowPlants()
        {

            // Get test weather data in case weather service is not available
            TestWeatherData twd = new TestWeatherData();
            WeatherModel testWeather = twd.GetTestWeatherData();

            // Get locations and weather for each location
            List<Location> locations = _context.Locations.ToList();
            Dictionary<int, WeatherModel> weathers = new Dictionary<int, WeatherModel>();
            await GetWeather(locations, weathers);
            
            // Grow each plant with weather data
            List<Plant> plants = _context.Plants.ToList();

            foreach (Plant plant in plants)
            {
                int locationId = plant.Planter.Garden.LocationId;
                if (weathers.ContainsKey(locationId)) { 
                    System.Diagnostics.Debug.WriteLine(string.Format("Growing plant {0}: {1} using live weather data from {2}",
                        plant.Id, plant.PlantType.Name, plant.Planter.Garden.Location.Name));
                    GrowPlant(plant, weathers[locationId]); 
                }
                else { 
                    System.Diagnostics.Debug.WriteLine(string.Format("Growing plant {0}: {1} using test weather data", 
                        plant.Id, plant.PlantType.Name));
                    GrowPlant(plant, testWeather); 
                }
            }

        }

        // Get weather data for each location
        public async Task GetWeather(List<Location> locations, Dictionary<int, WeatherModel> weathers)
        {
            string uri = "https://cs361-weather-service.herokuapp.com/history?";

            foreach (Location loc in locations)
            {
                System.Diagnostics.Debug.WriteLine("Fetching weather data from weather microservice.");
                string fullUri = uri + "latitude=" + loc.LatLon.Latitude + "&longitude=" + loc.LatLon.Longitude + "&time=" + (DateTimeOffset.Now.ToUnixTimeSeconds() - 86400);

                HttpResponseMessage response = await client.GetAsync(fullUri);

                if (response.IsSuccessStatusCode)
                {
                    var content = response.Content;
                    string jsonContent = content.ReadAsStringAsync().Result;

                    try
                    {
                        WeatherModel weather = JsonConvert.DeserializeObject<WeatherModel>(jsonContent);
                        System.Diagnostics.Debug.WriteLine(string.Format("Retrieved weather for {0}", loc.Name));
                        weathers.Add(loc.Id, weather);
                    }
                    catch (JsonSerializationException ex)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Problem deserializing json to weather model: {0}", ex.ToString()));
                    }
                }

            }
        }

        // Grow each plant
        public void GrowPlant(Plant plant, WeatherModel tw)
        {
            int currentDaySun = CalculateSun(tw);
            int currentDayWater = CalculateWater(tw);

            UpdateWaterAndSunLevels(plant, currentDaySun, currentDayWater);

            // Check if plant meets sun and water requirement to determine growth
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

        // Update water and sun levels for a single plant
        public void UpdateWaterAndSunLevels(Plant plant, int currentDaySun, int currentDayWater)
        {
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
        }

        // Determine sun level from weather data
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

        // Determine water level from weather data
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