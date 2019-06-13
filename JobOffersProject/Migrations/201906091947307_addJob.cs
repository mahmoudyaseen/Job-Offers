namespace JobOffersProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addJob : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Jobs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        JobName = c.String(nullable: false),
                        JobDescription = c.String(nullable: false),
                        JobImage = c.String(nullable: false),
                        CategoriesId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoriesId, cascadeDelete: true)
                .Index(t => t.CategoriesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "CategoriesId", "dbo.Categories");
            DropIndex("dbo.Jobs", new[] { "CategoriesId" });
            DropTable("dbo.Jobs");
        }
    }
}
