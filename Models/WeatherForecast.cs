using System;

namespace Projekt_RESTfulWebAPI
{
    public class WeatherForecast
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Schorching"
        };

        public static WeatherForecast GetRandomForecast()
        {
            var rng = new Random();

            WeatherForecast result = new WeatherForecast()
            {
                Date = DateTime.Now.AddDays(rng.Next(1, 8)),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            };

            return result;
        }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
