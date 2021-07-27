using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VirtualGarden.Models
{
    public class Garden
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public virtual List<Planter> Planters { get; set; }
    }
}