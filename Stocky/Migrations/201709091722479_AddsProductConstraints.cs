namespace Stocky.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddsProductConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Name", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Products", "Sku", c => c.String(nullable: false, maxLength: 10));
            AlterColumn("dbo.Products", "Description", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Description", c => c.String());
            AlterColumn("dbo.Products", "Sku", c => c.String());
            AlterColumn("dbo.Products", "Name", c => c.String());
        }
    }
}
