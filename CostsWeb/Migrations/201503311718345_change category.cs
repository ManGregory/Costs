namespace CostsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changecategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Categories", "ParentId", "dbo.Categories");
            DropIndex("dbo.Categories", new[] { "ParentId" });
            AddColumn("dbo.CostsJournals", "SubCategoryId", c => c.Int());
            AddForeignKey("dbo.CostsJournals", "SubCategoryId", "dbo.Categories", "Id");
            DropColumn("dbo.Categories", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Categories", "ParentId", c => c.Int());
            DropForeignKey("dbo.CostsJournals", "CategoryId", "dbo.Categories");
            DropColumn("dbo.CostsJournals", "SubCategoryId");
            CreateIndex("dbo.Categories", "ParentId");
            AddForeignKey("dbo.Categories", "ParentId", "dbo.Categories", "Id");
        }
    }
}
