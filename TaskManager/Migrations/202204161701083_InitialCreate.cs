namespace TaskManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Dates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        TaskName = c.String(),
                        Important = c.Boolean(nullable: false),
                        DateID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Dates", t => t.DateID, cascadeDelete: true)
                .Index(t => t.DateID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "DateID", "dbo.Dates");
            DropIndex("dbo.Tasks", new[] { "DateID" });
            DropTable("dbo.Tasks");
            DropTable("dbo.Dates");
        }
    }
}
