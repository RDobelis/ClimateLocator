﻿using Newtonsoft.Json;

namespace ClimateLocator.Core.Models
{
    public class Weather
    {
        public int Id { get; set; }
        [JsonProperty("app_temp")]
        public double Temperature { get; set; }
        [JsonProperty("weather")]
        public WeatherDescription WeatherDescription { get; set; }
    }
}
