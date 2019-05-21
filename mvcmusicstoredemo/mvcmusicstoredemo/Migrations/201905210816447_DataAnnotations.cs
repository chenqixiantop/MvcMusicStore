namespace MvcMusicStoreDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Order", "FirstName", c => c.String(nullable: false, maxLength: 160));
            AlterColumn("dbo.Order", "LastName", c => c.String(nullable: false, maxLength: 160));
            AlterColumn("dbo.Order", "Address", c => c.String(nullable: false, maxLength: 70));
            AlterColumn("dbo.Order", "City", c => c.String(nullable: false));
            AlterColumn("dbo.Order", "State", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Order", "PostalCode", c => c.String(nullable: false));
            AlterColumn("dbo.Order", "Country", c => c.String(nullable: false, maxLength: 40));
            AlterColumn("dbo.Order", "Phone", c => c.String(nullable: false, maxLength: 11));
            AlterColumn("dbo.Order", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Order", "Email", c => c.String());
            AlterColumn("dbo.Order", "Phone", c => c.String());
            AlterColumn("dbo.Order", "Country", c => c.String());
            AlterColumn("dbo.Order", "PostalCode", c => c.String());
            AlterColumn("dbo.Order", "State", c => c.String());
            AlterColumn("dbo.Order", "City", c => c.String());
            AlterColumn("dbo.Order", "Address", c => c.String());
            AlterColumn("dbo.Order", "LastName", c => c.String());
            AlterColumn("dbo.Order", "FirstName", c => c.String());
        }
    }
}
