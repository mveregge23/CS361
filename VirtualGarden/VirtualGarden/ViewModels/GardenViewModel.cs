using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VirtualGarden.Models;

namespace VirtualGarden.ViewModels
{
    public class GardenViewModel
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public Boolean isNewGarden { get; set; }
        public List<PlanterViewModel> Planters { get; set; }
    }
}