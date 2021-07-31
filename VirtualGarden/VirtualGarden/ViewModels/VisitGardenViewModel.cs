using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VirtualGarden.ViewModels
{
    public class VisitGardenViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}