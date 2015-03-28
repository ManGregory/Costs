namespace CostsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcostsjournalmodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CostsJournals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        CategoryId = c.Int(),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CostsJournals", "CategoryId", "dbo.Categories");
            DropIndex("dbo.CostsJournals", new[] { "CategoryId" });
            DropTable("dbo.CostsJournals");
        }
    }
}
