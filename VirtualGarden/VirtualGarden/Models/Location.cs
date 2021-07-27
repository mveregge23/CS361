using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Spatial;

namespace VirtualGarden.Models
{
    public class Location
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public DbGeography LatLon { get; private set; }
    }
}