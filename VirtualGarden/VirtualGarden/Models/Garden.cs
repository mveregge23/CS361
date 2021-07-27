using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VirtualGarden.Models
{
    public class Garden
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int LocationId { get; set; }

        public virtual Location Location { get; set; }

        public virtual List<Planter> Planters { get; set; }
    }
}