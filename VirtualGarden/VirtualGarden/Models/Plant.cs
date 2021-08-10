using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VirtualGarden.Models
{
    public class Plant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Range(0, 100)]
        public int Growth { get; set; }

        [Required]
        [Range(0, 100)]
        public int Water { get; set; }

        [Required]
        [Range(0, 100)]
        public int Sun { get; set; }

        public virtual Planter Planter { get; set; }

        [Required]
        public int PlantTypeId { get; set; }

        [ForeignKey("PlantTypeId")]
        public virtual PlantType PlantType { get; set; }
    }
}