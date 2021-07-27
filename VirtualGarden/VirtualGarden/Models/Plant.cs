using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualGarden.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public int Growth { get; set; }
        public int Water { get; set; }
        public int Sun { get; set; }
        public int GrowthProgress { get; set; }
    }
}