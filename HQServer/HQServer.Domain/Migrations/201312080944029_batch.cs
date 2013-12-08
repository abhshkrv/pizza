namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class batch : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutletInventories", "temporaryStock", c => c.Int(nullable: false));
            AddColumn("dbo.OutletInventories", "afterUpdateStock", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutletInventories", "afterUpdateStock");
            DropColumn("dbo.OutletInventories", "temporaryStock");
        }
    }
}
