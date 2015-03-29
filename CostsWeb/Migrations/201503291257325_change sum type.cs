namespace CostsWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changesumtype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CostsJournals", "Sum", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CostsJournals", "Sum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
