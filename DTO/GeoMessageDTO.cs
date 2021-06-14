using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Projekt_RESTfulWebAPI.Models;

namespace Projekt_RESTfulWebAPI.DTO
{
    public class GeoMessageDTO
    {
        public string Message { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
