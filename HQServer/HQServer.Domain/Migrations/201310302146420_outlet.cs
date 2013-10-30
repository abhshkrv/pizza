namespace HQServer.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class outlet : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OutletInventories", "discountPercentage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OutletInventories", "discountPercentage");
        }
    }
}
