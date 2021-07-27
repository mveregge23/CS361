using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using VirtualGarden.Models;

namespace VirtualGarden.ViewModels
{
    public class GardenFormViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Location { get; set; }

        public IEnumerable<Location> Locations { get; set; }
    }
}