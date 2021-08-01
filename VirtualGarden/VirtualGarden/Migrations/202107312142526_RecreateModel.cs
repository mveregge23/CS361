namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RecreateModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gardens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LatLon = c.Geography(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Planters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GardenId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gardens", t => t.GardenId, cascadeDelete: true)
                .Index(t => t.GardenId);
            
            CreateTable(
                "dbo.Plants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Growth = c.Int(nullable: false),
                        Water = c.Int(nullable: false),
                        Sun = c.Int(nullable: false),
                        GrowthProgress = c.Int(nullable: false),
                        PlantTypeId = c.Int(nullable: false),
                        Planter_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PlantTypes", t => t.PlantTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Planters", t => t.Planter_Id)
                .Index(t => t.PlantTypeId)
                .Index(t => t.Planter_Id);
            
            CreateTable(
                "dbo.PlantTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        WaterRequirement = c.Single(nullable: false),
                        SunRequirement = c.Single(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Planters", "GardenId", "dbo.Gardens");
            DropForeignKey("dbo.Plants", "Planter_Id", "dbo.Planters");
            DropForeignKey("dbo.Plants", "PlantTypeId", "dbo.PlantTypes");
            DropForeignKey("dbo.Gardens", "LocationId", "dbo.Locations");
            DropIndex("dbo.Plants", new[] { "Planter_Id" });
            DropIndex("dbo.Plants", new[] { "PlantTypeId" });
            DropIndex("dbo.Planters", new[] { "GardenId" });
            DropIndex("dbo.Gardens", new[] { "LocationId" });
            DropTable("dbo.PlantTypes");
            DropTable("dbo.Plants");
            DropTable("dbo.Planters");
            DropTable("dbo.Locations");
            DropTable("dbo.Gardens");
        }
    }
}
