using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    class ApiService
    {
        public static async Task<Root?> GetWeather(double latitude, double longitude)
        {
            var client = new HttpClient();
            var url = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&appid=faeb14dd3f0d880980cf2137b29720e6";
            Console.WriteLine($"Request URL: {url}"); // Log the URL
            try
            {
                var response = await client.GetStringAsync(url);
                Console.WriteLine(response); // Log the JSON response
                return JsonConvert.DeserializeObject<Root>(response);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

        public static async Task<Root?> GetWeatherByCity(string city)
        {
            var httpClient = new HttpClient();
            var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&appid=faeb14dd3f0d880980cf2137b29720e6";
            Console.WriteLine($"Request URL: {url}"); // Log the URL
            try
            {
                var response = await httpClient.GetStringAsync(url);
                return JsonConvert.DeserializeObject<Root>(response);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return null;
            }
        }

    }
}
