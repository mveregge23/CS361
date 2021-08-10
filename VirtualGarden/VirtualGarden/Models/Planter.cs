using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualGarden.Models
{
    public class Planter
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GardenId { get; set; }

        [ForeignKey("GardenId")]
        public virtual Garden Garden { get; set; }

        public virtual Plant Plant { get; set; }
    }
}