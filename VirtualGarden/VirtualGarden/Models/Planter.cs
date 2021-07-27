using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualGarden.Models
{
    public class Planter
    {
        public int Id { get; set; }
        public Garden Garden { get; set; }
        public Plant Plant { get; set; }
    }
}