namespace CostsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIsDeletedfield : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.CostsJournals", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CostsJournals", "IsDeleted");
            DropColumn("dbo.Categories", "IsDeleted");
        }
    }
}
