using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VirtualGarden.Models
{
    public class Plant
    {
        public int Id { get; set; }

        [Required]
        public int Growth { get; set; }

        [Required]
        public int Water { get; set; }

        [Required]
        public int Sun { get; set; }

        [Required]
        public int GrowthProgress { get; set; }

        public virtual Planter Planter { get; set; }

        public virtual PlantType PlantType { get; set; }

        [Required]
        public int PlantTypeId { get; set; }
    }
}