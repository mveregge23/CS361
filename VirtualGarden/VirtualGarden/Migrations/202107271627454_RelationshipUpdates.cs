namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationshipUpdates : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Planters", "Garden_Name", "dbo.Gardens");
            DropIndex("dbo.Planters", new[] { "Garden_Name" });
            RenameColumn(table: "dbo.Planters", name: "Garden_Name", newName: "Garden_Id");
            DropPrimaryKey("dbo.Gardens");
            AlterColumn("dbo.Gardens", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Gardens", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Planters", "Garden_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Gardens", "Id");
            CreateIndex("dbo.Planters", "Garden_Id");
            AddForeignKey("dbo.Planters", "Garden_Id", "dbo.Gardens", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Planters", "Garden_Id", "dbo.Gardens");
            DropIndex("dbo.Planters", new[] { "Garden_Id" });
            DropPrimaryKey("dbo.Gardens");
            AlterColumn("dbo.Planters", "Garden_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Gardens", "Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Gardens", "Name", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.Gardens", "Name");
            RenameColumn(table: "dbo.Planters", name: "Garden_Id", newName: "Garden_Name");
            CreateIndex("dbo.Planters", "Garden_Name");
            AddForeignKey("dbo.Planters", "Garden_Name", "dbo.Gardens", "Name", cascadeDelete: true);
        }
    }
}
