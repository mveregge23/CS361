namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        LatLon = c.Geography(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Gardens", "Id");
            AddForeignKey("dbo.Gardens", "Id", "dbo.Locations", "Id", cascadeDelete: false);
            DropColumn("dbo.Gardens", "Location");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gardens", "Location", c => c.Geography(nullable: false));
            DropForeignKey("dbo.Gardens", "Id", "dbo.Locations");
            DropIndex("dbo.Gardens", new[] { "Id" });
            DropTable("dbo.Locations");
        }
    }
}
