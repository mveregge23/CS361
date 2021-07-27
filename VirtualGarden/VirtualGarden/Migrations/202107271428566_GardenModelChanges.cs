namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Spatial;
    
    public partial class GardenModelChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Planters", "Garden_Id", "dbo.Gardens");
            DropIndex("dbo.Planters", new[] { "Garden_Id" });
            RenameColumn(table: "dbo.Planters", name: "Garden_Id", newName: "Garden_Name");
            DropPrimaryKey("dbo.Gardens");
            AddColumn("dbo.Gardens", "Name", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Gardens", "Location", c => c.Geography(nullable: false));
            AlterColumn("dbo.Gardens", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Planters", "Garden_Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Gardens", "Name");
            CreateIndex("dbo.Planters", "Garden_Name");
            AddForeignKey("dbo.Planters", "Garden_Name", "dbo.Gardens", "Name", cascadeDelete: true);
            DropColumn("dbo.Gardens", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Gardens", "Code", c => c.String());
            DropForeignKey("dbo.Planters", "Garden_Name", "dbo.Gardens");
            DropIndex("dbo.Planters", new[] { "Garden_Name" });
            DropPrimaryKey("dbo.Gardens");
            AlterColumn("dbo.Planters", "Garden_Name", c => c.Int(nullable: false));
            AlterColumn("dbo.Gardens", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Gardens", "Location");
            DropColumn("dbo.Gardens", "Name");
            AddPrimaryKey("dbo.Gardens", "Id");
            RenameColumn(table: "dbo.Planters", name: "Garden_Name", newName: "Garden_Id");
            CreateIndex("dbo.Planters", "Garden_Id");
            AddForeignKey("dbo.Planters", "Garden_Id", "dbo.Gardens", "Id", cascadeDelete: true);
        }
    }
}
