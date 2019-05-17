namespace mvcmusicstoredemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Album", "Title", c => c.String(nullable: false, maxLength: 160));
            AlterColumn("dbo.Album", "AlbumArtUrl", c => c.String(maxLength: 1024));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Album", "AlbumArtUrl", c => c.String());
            AlterColumn("dbo.Album", "Title", c => c.String());
        }
    }
}
