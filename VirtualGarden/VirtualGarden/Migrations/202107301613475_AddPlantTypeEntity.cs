namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPlantTypeEntity : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.Plants", "PlantTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Plants", "PlantTypeId");
            AddForeignKey("dbo.Plants", "PlantTypeId", "dbo.PlantTypes", "Id", cascadeDelete: true);
            DropColumn("dbo.Plants", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Plants", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.Plants", "PlantTypeId", "dbo.PlantTypes");
            DropIndex("dbo.Plants", new[] { "PlantTypeId" });
            DropColumn("dbo.Plants", "PlantTypeId");
            DropTable("dbo.PlantTypes");
        }
    }
}
