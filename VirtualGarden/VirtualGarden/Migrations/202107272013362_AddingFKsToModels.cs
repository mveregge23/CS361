namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingFKsToModels : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Planters", name: "Garden_Id", newName: "GardenId");
            RenameIndex(table: "dbo.Planters", name: "IX_Garden_Id", newName: "IX_GardenId");
            AddColumn("dbo.Plants", "PlanterId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Plants", "PlanterId");
            RenameIndex(table: "dbo.Planters", name: "IX_GardenId", newName: "IX_Garden_Id");
            RenameColumn(table: "dbo.Planters", name: "GardenId", newName: "Garden_Id");
        }
    }
}
