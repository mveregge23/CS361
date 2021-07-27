using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace VirtualGarden.Models
{
    public class ApplicationDbContext: DbContext
    {
        public DbSet<Garden> Gardens { get; set; }
        public DbSet<Planter> Planters { get; set; }
        public DbSet<Plant> Plants { get; set; }
        public DbSet<Location> Locations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Garden>().HasMany(g => g.Planters).WithRequired(p => p.Garden);
            modelBuilder.Entity<Garden>().HasRequired(g => g.Location);
            modelBuilder.Entity<Planter>().HasOptional(p => p.Plant).WithOptionalPrincipal(p => p.Planter);
        }
    }
}