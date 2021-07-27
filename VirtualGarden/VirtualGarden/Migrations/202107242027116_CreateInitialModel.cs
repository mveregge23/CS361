namespace VirtualGarden.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInitialModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gardens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Planters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Garden_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Gardens", t => t.Garden_Id, cascadeDelete: true)
                .Index(t => t.Garden_Id);
            
            CreateTable(
                "dbo.Plants",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Growth = c.Int(nullable: false),
                        Water = c.Int(nullable: false),
                        Sun = c.Int(nullable: false),
                        GrowthProgress = c.Int(nullable: false),
                        Planter_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Planters", t => t.Planter_Id)
                .Index(t => t.Planter_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Planters", "Garden_Id", "dbo.Gardens");
            DropForeignKey("dbo.Plants", "Planter_Id", "dbo.Planters");
            DropIndex("dbo.Plants", new[] { "Planter_Id" });
            DropIndex("dbo.Planters", new[] { "Garden_Id" });
            DropTable("dbo.Plants");
            DropTable("dbo.Planters");
            DropTable("dbo.Gardens");
        }
    }
}
