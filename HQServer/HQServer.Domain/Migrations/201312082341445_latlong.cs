namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class latlong : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Outlets", "latitude", c => c.Double(nullable: false));
            AddColumn("dbo.Outlets", "longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Outlets", "longitude");
            DropColumn("dbo.Outlets", "latitude");
        }
    }
}
