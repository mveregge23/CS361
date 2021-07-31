using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VirtualGarden.Models
{
    public class PlantType
    {
        public int Id { get; set; }
        
        [Required]
        public int Type { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public float WaterRequirement { get; set; }

        [Required]
        public float SunRequirement { get; set; }
    }
}