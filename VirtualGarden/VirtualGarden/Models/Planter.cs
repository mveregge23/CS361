using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VirtualGarden.Models
{
    public class Planter
    {
        public int Id { get; set; }

        [Required]
        public int GardenId { get; set; }

        public virtual Plant Plant { get; set; }
    }
}