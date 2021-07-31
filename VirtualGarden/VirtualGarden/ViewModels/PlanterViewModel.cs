using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtualGarden.Models;

namespace VirtualGarden.ViewModels
{
    public class PlanterViewModel
    {
        public int PlanterId { get; set; }
        public Plant Plant { get; set; }
    };
}