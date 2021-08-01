﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtualGarden.Models;

namespace VirtualGarden.ViewModels
{
    public class PlanterViewModel
    {
        public IEnumerable<PlantTypeViewModel> PlantTypes { get; set; }
        public int PlantTypeId { get; set; }
        public string PlantTypeName { get; set; }
        public int PlanterId { get; set; }
    };
}