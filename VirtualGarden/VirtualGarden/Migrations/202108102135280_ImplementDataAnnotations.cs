namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImplementDataAnnotations : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Planters", "GardenId", "dbo.Gardens");
            AlterColumn("dbo.Gardens", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Locations", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Locations", "LatLon", c => c.Geography(nullable: false));
            AlterColumn("dbo.PlantTypes", "Name", c => c.String(nullable: false, maxLength: 100));
            AddForeignKey("dbo.Planters", "GardenId", "dbo.Gardens", "Id");
            DropColumn("dbo.Plants", "GrowthProgress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Plants", "GrowthProgress", c => c.Int(nullable: false));
            DropForeignKey("dbo.Planters", "GardenId", "dbo.Gardens");
            AlterColumn("dbo.PlantTypes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Locations", "LatLon", c => c.Geography());
            AlterColumn("dbo.Locations", "Name", c => c.String());
            AlterColumn("dbo.Gardens", "Name", c => c.String(nullable: false));
            AddForeignKey("dbo.Planters", "GardenId", "dbo.Gardens", "Id", cascadeDelete: true);
        }
    }
}
