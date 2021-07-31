﻿using System;
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

        public Planter Planter { get; set; }

        public int PlanterId { get; set; }

        [Required]
        public PlantType PlantType { get; set; }

        [Required]
        public int PlantTypeId { get; set; }
    }
}