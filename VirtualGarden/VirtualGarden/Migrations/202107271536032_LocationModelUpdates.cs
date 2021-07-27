namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LocationModelUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Gardens", "Id", "dbo.Locations");
            DropIndex("dbo.Gardens", new[] { "Id" });
            AddColumn("dbo.Gardens", "LocationId", c => c.Int(nullable: false));
            CreateIndex("dbo.Gardens", "LocationId");
            AddForeignKey("dbo.Gardens", "LocationId", "dbo.Locations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Gardens", "LocationId", "dbo.Locations");
            DropIndex("dbo.Gardens", new[] { "LocationId" });
            DropColumn("dbo.Gardens", "LocationId");
            CreateIndex("dbo.Gardens", "Id");
            AddForeignKey("dbo.Gardens", "Id", "dbo.Locations", "Id", cascadeDelete: true);
        }
    }
}
