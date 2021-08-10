using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VirtualGarden.Models
{
    public class PlantType
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        [Range(0, 100)]
        public int Type { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [Range(0, 1)]
        public float WaterRequirement { get; set; }

        [Required]
        [Range(0, 1)]
        public float SunRequirement { get; set; }
    }
}